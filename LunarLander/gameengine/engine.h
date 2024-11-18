#pragma once

#include <gameobject.h>
#include <camera.h>

namespace game {

class GameEngine {
    std::vector<GameObject*> gameObjects;

public:
    Camera camera;
    
    GameEngine (Camera&& camera) : camera(camera) {}

    GameEngine (const GameEngine&)              = delete;
    GameEngine (GameEngine&&)                   = delete;
    GameEngine& operator= (const GameEngine&)   = delete;
    GameEngine& operator= (GameEngine&&)        = delete;
    
    ~GameEngine () = default;

    void update (float deltatime) {
        for (auto gameObject : gameObjects) gameObject->update(deltatime);
    }
    GameEngine& add (GameObject* gameObject) {
        gameObjects.push_back(gameObject);
        gameObject->awake();
        gameObject->start();

        return *this;
    }
    void render () {
        camera.clear();
        for (auto gameObject : gameObjects) gameObject->render(camera);
    }
};

}