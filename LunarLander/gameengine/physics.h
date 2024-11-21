#pragma once

#include <geometry.h>
#include <cstdint>

namespace game {

struct Transform {
    geom::Rotation rotation;
    geom::Vector2f position;
};

struct PhysicsBehavour {
    uint32_t layerMask;
    Transform* gameObjectTransform;
    float mass;
    
    
};

struct PolygonCollider {
    /* data */
};


}