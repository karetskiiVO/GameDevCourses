#pragma once

#include <geometry.h>
#include <cstdint>

namespace game {

struct Transform {
    geom::Rotation rotation;
    geom::Vector2f position;
};

class PhysicsBehavour {
    uint32_t layerMask = 0;
    bool active = false;
    bool triggered = false;
    
    Transform* gameObjectTransform = nullptr;
    float mass, inertion;

    geom::Vector2f velocity;
    float rotationvelocity = 0.0f;

    std::vector<geom::Polygon> colliders;  
public:
    PhysicsBehavour () {}

    PhysicsBehavour (
        const geom::Polygon& colider, uint32_t layer, float mass, float inertion, bool active
    ) : layerMask(layer), mass(mass), inertion(inertion), active(active) {
        colliders = geom::splitToConvex(colider);
    }

    void force (geom::Point point, geom::Vector2f f, float delatatime) {
        if (!active) return;

        velocity += f / mass * delatatime;
        rotationvelocity += geom::cross(point - gameObjectTransform->position, f) / inertion * delatatime;
    }

    const Transform* getTransform () const {
        return gameObjectTransform;
    }

    bool isTriggered () const {
        return triggered;
    }

    void physicsUpdate (float delatatime) {
        if (!active) return;

        gameObjectTransform->position += velocity * delatatime;
        gameObjectTransform->rotation += geom::Rotation(rotationvelocity * delatatime); 
    }

    friend class GameEngine;
    friend struct GameObject;
};


struct IntersectionInfo {
    geom::Vector2f position;
    geom::Vector2f axis;
    float dist;
};

IntersectionInfo& polygonIntersection (
    const geom::Polygon& fst, const Transform& trfst, const geom::Polygon& snd, const Transform& trsnd, IntersectionInfo& out
) {
    const geom::Rotation rot = geom::Rotation(std::acos(0.f));

    struct LineSegment {
        float min, max;
        geom::Point pointmax, pointmin;
    };

    auto separateAxis = [] (const geom::Polygon& poly, const Transform& tr, geom::Vector2f axis) -> LineSegment {
        auto res = LineSegment{
             std::numeric_limits<float>::infinity(),
            -std::numeric_limits<float>::infinity()
        };
        
        // тернарник

        for (auto vert : poly.verticies) {
            vert = tr.rotation*vert + tr.position;

            if (res.min > geom::dot<float>(vert, axis)) {
                res.min = geom::dot<float>(vert, axis);
                res.pointmin = vert;
            }

            if (res.max < geom::dot<float>(vert, axis)) {
                res.max = geom::dot<float>(vert, axis);
                res.pointmax = vert;
            }
        }

        return res;
    };

    out = IntersectionInfo{
        geom::Vector2f(),
        geom::Vector2f(),
        -std::numeric_limits<float>::infinity()
    };

    for (size_t i = 0; i < fst.verticies.size(); i++) {
        auto axis = (rot + trfst.rotation) * (fst.verticies[(i+1) % fst.verticies.size()] - fst.verticies[i]);
        axis /= axis.magnitude();

        auto seg1 = separateAxis(fst, trfst, axis);
        auto seg2 = separateAxis(snd, trsnd, axis);

        if (seg1.max < seg2.max) std::swap(seg1, seg2);

        auto dist = (std::max(seg1.max, seg2.max) - std::min(seg1.min, seg2.min)) - (seg1.max - seg1.min) - (seg2.max - seg2.min);
        if (dist > 0) {
            out = {geom::Vector2f(), geom::Vector2f(), std::numeric_limits<float>::infinity()};
            return out;
        }

        if (dist > out.dist) out = {seg1.pointmin, axis, dist};
    }

    for (size_t i = 0; i < snd.verticies.size(); i++) {
        auto axis = (- rot + trsnd.rotation) * (snd.verticies[(i+1) % snd.verticies.size()] - snd.verticies[i]);
        axis /= axis.magnitude();

        auto seg1 = separateAxis(fst, trfst, axis);
        auto seg2 = separateAxis(snd, trsnd, axis);

        if (seg1.max < seg2.max) std::swap(seg1, seg2);

        auto dist = (std::max(seg1.max, seg2.max) - std::min(seg1.min, seg2.min)) - (seg1.max - seg1.min) - (seg2.max - seg2.min);
        if (dist > 0) {
            out = {geom::Vector2f(), geom::Vector2f(), std::numeric_limits<float>::infinity()};
            return out;
        }

        if (dist > out.dist) out = {seg1.pointmin, axis, dist};
    }

    return out;
}

}