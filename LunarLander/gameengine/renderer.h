#pragma once

#include <camera.h>

namespace game {

struct Renderer {
    virtual void render (Camera& camera, const Transform& gameObjectTransform) {}
};

}