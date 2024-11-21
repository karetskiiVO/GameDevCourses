#pragma once

#include <random>
#include <geometry.h>
#include <gameengine.h>

game::GameObject* CreateMoon (float R) {
    geom::Polygon poly({});

    size_t maxIter = 24;
    size_t segments = 128;

    poly.verticies.resize(segments);

    std::random_device randomDevice;
    std::normal_distribution<float> distr(R, R/6);

    for (size_t iter = 0; iter < maxIter; iter++) {
        for (size_t i = 0; i < segments; i++) {
            poly.verticies[i] += geom::Rotation(i * 1.0f / segments * 4 * acosf(0)) * geom::Vector2f{1.0f, 0.0f} * distr(randomDevice) / maxIter;
        }
    }

    return new game::GameObject{
        game::Transform{},
        //new game::PolygonRenderer(poly),
        (new game::MultiRenderer())->addPolygons(geom::splitToConvex(poly)),
        game::PhysicsBehavour{
            poly,
            uint32_t(-1),
            10.0f, 10.0f,
            false
        }
    };
}

