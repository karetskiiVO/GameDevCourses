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
    float mass, inertion;
    bool active;
    std::vector<geom::Polygon> colliders;  
    
    void force (geom::Point point, geom::Vector2f vector) {
        
    }
};

}