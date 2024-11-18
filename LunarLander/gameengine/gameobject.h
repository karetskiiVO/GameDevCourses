#pragma once

#include <vector>

#include <camera.h>
#include <physics.h>
#include <geometry.h>
#include <renderer.h>
#include <component.h>

namespace game {

struct GameObject {
    Transform transform;
    Renderer* renderer;

    std::vector<Component*> components;

    GameObject(Transform tranform, Renderer* renderer) : transform(tranform), renderer(renderer) {}

    void render  (Camera& camera) {
        renderer->render(camera, transform);
    }

    void awake   () {
        for (auto component : components) component->awake();
    }
    void start   () {
        for (auto component : components) component->start();
    }
    void update (float deltatime) {
        for (auto component : components) component->update(deltatime);
    }
    void destroy () {
        for (auto component : components) component->destroy();
    }
};

}