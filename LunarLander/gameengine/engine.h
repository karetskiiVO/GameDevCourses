#pragma once

#include <gameobject.h>
#include <camera.h>
#include <string>

std::string debugLog = "";

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
        const geom::Rotation rot = geom::Rotation(std::acos(0.f));

        for (size_t fst = 0; fst < gameObjects.size(); fst++) {
            for (size_t snd = fst + 1; snd < gameObjects.size(); snd++) {
                auto& transform1 = gameObjects[fst]->transform;
                auto& transform2 = gameObjects[snd]->transform;

                auto& behavour1 = gameObjects[fst]->physicsBehavour;
                auto& behavour2 = gameObjects[snd]->physicsBehavour;

                if ((!behavour1.active) && (!behavour2.active)) continue;
                if (!(behavour1.layerMask & behavour2.layerMask)) continue;

                for (const auto& collider1 : behavour1.colliders) {
                    for (const auto& collider2 : behavour2.colliders) {
                        IntersectionInfo info;

                        polygonIntersection(collider1, transform1, collider2, transform2, info);

                        if (info.dist < 0) {
                            debugLog = std::to_string(info.axis.x) + " " + std::to_string(info.axis.y) + " " 
                                     + std::to_string(info.position.x) + " " + std::to_string(info.position.y);

                            
                            if (behavour1.active) {
                                behavour1.force(
                                    info.position,
                                    -info.axis * 10000 * info.dist,
                                    deltatime
                                );
                            }
                            if (behavour2.active) {
                                behavour2.force(
                                    info.position,
                                    info.axis * 10000 * (-info.dist),
                                    deltatime
                                );
                            }
                        }
                    }
                }
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