#pragma once

#include <geometry.h>
#include <cstdint>

namespace game {

struct Transform {
    geom::Rotation rotation;
    geom::Vector2f position;
};

struct PhysicsBehavour {
    uint32_t layerMask = 0;
    bool active = false;
    
    Transform* gameObjectTransform = nullptr;
    float mass, inertion;

    geom::Vector2f velocity;
    float rotationvelocity = 0.0f;

    std::vector<geom::Polygon> colliders;  
    
    PhysicsBehavour () {}

    PhysicsBehavour (
        const geom::Polygon& colider, uint32_t layer, float mass, float inertion, bool active
    ) : layerMask(layer), mass(mass), inertion(inertion), active(active) {
        colliders = geom::splitToConvex(colider);
    }

    void force (geom::Point point, geom::Vector2f f, float delatatime) {
        if (!active) return;

        velocity += f / mass * delatatime;
    }

    void physicsUpdate (float delatatime) {
        if (!active) return;

        gameObjectTransform->position += velocity * delatatime;
        // gameObjectTransform->rotation += geom::Rotation(rotationvelocity * delatatime); 
    }
};

}