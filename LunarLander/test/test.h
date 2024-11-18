#pragma once

#include <gameengine.h>

struct PointRenderer : public game::Renderer {
    virtual void render (game::Camera& camera, const game::Transform& gameObjectTransform) {
        camera.drawPoint(gameObjectTransform.position, game::RGBColor(255, 255, 255), 1);
    }
};

struct Mover : public game::Component {
    game::Transform* parentTransform = nullptr;
    geom::Vector2f velocity;

    Mover (game::Transform* parentTransform, geom::Vector2f velocity) :
        parentTransform(parentTransform), velocity(velocity) {}

    void update (float deltatime) {
        parentTransform->position += velocity * deltatime;
    }
};