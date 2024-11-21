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

std::vector<Polygon> splitToConvex (Polygon poly) {
    std::vector<Polygon> polygons;

    const std::function<void(std::vector<geom::Point>)> createConvex = [&polygons, &createConvex] (std::vector<geom::Point>&& points) {
        if (points.size() < 3) return;
        if (points.size() == 3) {
            polygons.push_back(points);
            return;
        }

        std::vector<geom::Point> buf;
        std::vector<geom::Point> res;

        res.push_back(points[0]);
        res.push_back(points[1]);

        auto prevEdge = points[1] - points[0];

        for (size_t i = 2; i < points.size(); i++) {
            auto point = points[i]; 
            auto currEdge = point - res.back();

            if (cross(currEdge, prevEdge) < -1e-2) {
                buf.push_back(point);
            } else {
                if (buf.size() > 0) {
                    buf.push_back(res.back());

                    createConvex(buf);
                    buf.clear();
                }

                res.push_back(point);

                prevEdge = currEdge;
            }
        }

        if (buf.size() > 0) {
            buf.push_back(res[0]);
            buf.push_back(res.back());

            createConvex(buf);
        }

        if (res.size() < 3) return;
        polygons.push_back(res);
    };

    createConvex(poly.verticies);

    return polygons;
}

}