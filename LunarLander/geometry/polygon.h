#pragma once

#include <vector>
#include <algorithm>
#include <limits>
#include <numbers>
#include <functional>

#include <rotation.h>
#include <vector2.h>

namespace geom {

struct Polygon {
    std::vector<Vector2f> verticies;

    Polygon (const std::vector<Vector2f>& verticies) : verticies(verticies) {}
};

std::pair<Vector2f, float> polygonIntersection (const Polygon& fst, const Polygon& snd) {
    const Rotation rot = Rotation(std::acos(0.f));

    struct LineSegment {
        float min, max;
    };

    auto separateAxis = [] (const Polygon& poly, Vector2f axis) -> LineSegment {
        auto res = LineSegment{
            -std::numeric_limits<float>::infinity(),
             std::numeric_limits<float>::infinity()
        };
        
        // тернарник

        for (const auto& vert : poly.verticies) {
            res.min = std::min(res.min, dot<float>(vert, axis));
            res.max = std::max(res.max, dot<float>(vert, axis));
        }

        return res;
    };

    auto res = std::pair<Vector2f, float>{
        Vector2f(),
        std::numeric_limits<float>::infinity()
    };

    for (size_t i = 0; i < fst.verticies.size(); i++) {
        auto axis = rot * (fst.verticies[(i+1) % fst.verticies.size()] - fst.verticies[i]);
        axis /= axis.magnitude();

        auto seg1 = separateAxis(fst, axis);
        auto seg2 = separateAxis(snd, axis);

        auto dist = (std::max(seg1.max, seg2.max) - std::min(seg1.min, seg2.min)) - (seg1.max - seg1.min) - (seg2.max - seg2.min);
        if (dist > 0) return {Vector2f(), std::numeric_limits<float>::infinity()};

        if (dist > res.second) res = {axis, dist};
    }

    for (size_t i = 0; i < snd.verticies.size(); i++) {
        auto axis = rot * (snd.verticies[(i+1) % snd.verticies.size()] - snd.verticies[i]);
        axis /= axis.magnitude();

        auto seg1 = separateAxis(fst, axis);
        auto seg2 = separateAxis(snd, axis);

        auto dist = (std::max(seg1.max, seg2.max) - std::min(seg1.min, seg2.min)) - (seg1.max - seg1.min) - (seg2.max - seg2.min);
        if (dist > 0) return {Vector2f(), std::numeric_limits<float>::infinity()};

        if (dist > res.second) res = {axis, dist};
    }

    return res;
}

std::vector<Polygon> splitToConvex (Polygon poly) {
    std::vector<Polygon> res;

    poly.verticies.push_back(poly.verticies[0]);

    const std::function<void(size_t, size_t)> createConvex = [&poly, &res, &createConvex] (size_t begin, size_t end) {
        if ((end - begin + poly.verticies.size()) % poly.verticies.size() <= 1) return;

        if ((end - begin + poly.verticies.size()) % poly.verticies.size() == 2) {
            res.push_back(Polygon{{poly.verticies.begin() + begin, poly.verticies.begin() + end}});
            return;
        }

        std::vector<Point> current = {poly.verticies[begin], poly.verticies[(begin + 1) % poly.verticies.size()]};

        Vector2f prevEdge = current[1] - current[0];
        size_t prevVert = (begin + 1) % poly.verticies.size();
        for (size_t newVert = begin + 2;; newVert = (newVert + 1) % poly.verticies.size()) {
            Vector2f currEdge = poly.verticies[newVert] - current.back();

            if (cross(currEdge, prevEdge) < 0) continue;
            
            createConvex(prevVert, newVert);
            current.push_back(poly.verticies[newVert]);
            
            prevEdge = currEdge;
            prevVert = newVert;

            if (newVert == end) break;
        }

        createConvex(prevVert, end);

        if ((current[0] - current.back()).magnitude2() < 1e-7) current.pop_back();
        res.push_back(current);
    };

    createConvex(0, poly.verticies.size() - 1);

    return res;
}

}