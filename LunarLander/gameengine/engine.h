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

    void physicsUpdate (float deltatime) {
        for (size_t fst = 0; fst < gameObjects.size(); fst++) {
            for (size_t snd = fst + 1; snd < gameObjects.size(); snd++) {
                auto& behavour1 = gameObjects[fst]->physicsBehavour;
                auto& behavour2 = gameObjects[snd]->physicsBehavour;

                if (!(behavour1.active || behavour2.active)) continue;
                if (!(behavour1.layerMask & behavour2.layerMask)) continue;

                //for()
            }
        }

        for (auto gameObject : gameObjects) gameObject->physicsBehavour.physicsUpdate(deltatime);
    }
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