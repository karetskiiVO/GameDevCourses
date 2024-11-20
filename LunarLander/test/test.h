#pragma once

#include <gameengine.h>

struct PointRenderer : public game::Renderer {
    void render (game::Camera& camera, const game::Transform& gameObjectTransform) {
        camera.drawCircle(gameObjectTransform.position, game::RGBColor(255, 255, 255), 1);
    }
};

struct SegmentRenderer : public game::Renderer {
    geom::Vector2f begin, end;

    SegmentRenderer (geom::Vector2f begin, geom::Vector2f end) : begin(begin), end(end) {}

    void render (game::Camera& camera, const game::Transform& gameObjectTransform) {
        camera.drawSegment(
            gameObjectTransform.position + begin,
            gameObjectTransform.position + end,
            game::RGBColor(255, 255, 255),
            1
        );
    }
};

struct Mover : public game::Component {
    game::Transform* parentTransform = nullptr;
    geom::Vector2f velocity;

    Mover (game::Transform* parentTransform, geom::Vector2f velocity) :
        parentTransform(parentTransform), velocity(velocity) {}

    void update (float deltatime) {
        parentTransform->rotation += geom::Rotation(deltatime);
    }
};