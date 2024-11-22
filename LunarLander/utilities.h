#pragma once

#include <random>

#include "Engine.h"

#include <geometry.h>
#include <gameengine.h>

game::GameObject* createPlanet (const game::Transform& transform, float R) {
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
        transform,
        new game::PolygonRenderer(poly),
        game::PhysicsBehavour{
            poly,
            1,
            10.0f, 10.0f,
            false
        }
    };
}

game::GameObject* createLander (const game::Transform& transform) {
    geom::Polygon cockpit{{
        {-0.5f,  1.0f},
        {-1.0f,  1.5f},
        {-1.0f,  2.0f},
        {-0.5f,  2.5f},
        { 0.5f,  2.5f},
        { 1.0f,  2.0f},
        { 1.0f,  1.5f},
        { 0.5f,  1.0f},
    }};
    geom::Polygon fueltank{{
        {-1.0f,  0.0f},
        {-1.5f,  0.5f},
        {-1.0f,  1.0f},
        { 1.0f,  1.0f},
        { 1.5f,  0.5f},
        { 1.0f,  0.0f},
    }};
    geom::Polygon jet{{
        {-0.4f, -0.4f},
        {-0.2f,  0.0f},
        { 0.2f,  0.0f},
        { 0.4f,  -0.4f},
    }};
    geom::Polygon leftsupport{{    
        {-0.5f,  1.0f},
        {-1.0f, -0.5f},
    }};
    geom::Polygon rightsupport{{    
        { 0.5f,  1.0f},
        { 1.0f, -0.5f},
    }};

    geom::Polygon collider{{
        {-1.0f, -0.5f},
        {-1.5f,  0.5f},
        {-1.0f,  2.0f},
        {-0.5f,  2.5f},

        { 0.5f,  2.5f},
        { 1.0f,  2.0f},
        { 1.5f,  0.5f},
        { 1.0f, -0.5f},
    }};

    return new game::GameObject{
        transform,
        (new game::MultiRenderer())->addPolygons({cockpit, fueltank, jet, leftsupport, rightsupport}),
        game::PhysicsBehavour{
            collider,
            1,
            10.0f, 100.0f,
            true,
            true
        }
    };
}

class GravicyMaker : public game::Component {
    game::PhysicsBehavour* behavour = nullptr;

public:
    GravicyMaker (game::GameObject* gameObject) : behavour(&gameObject->physicsBehavour) {}

    void update (float deltatime) {
        auto toCenter = geom::Vector2f(0, 0) - behavour->getTransform()->position;

        behavour->force(
            behavour->getTransform()->position, 
            40.0f * toCenter / toCenter.magnitude(), 
            deltatime
        );
    }
};

class CameraMover : public game::Component {
    game::Transform* objectTransform = nullptr;
    game::Transform* cameraTransform = nullptr;
public:
    CameraMover (game::Transform* objectTransform, game::Transform* cameraTransform) 
        : objectTransform(objectTransform), cameraTransform(cameraTransform) {}
    
    void update (float) {
        cameraTransform->position = objectTransform->position;
        if (objectTransform->position.magnitude2() < 1e-3) return;
        cameraTransform->rotation = geom::Rotation(objectTransform->position) - geom::Rotation(acosf(0));
    }
};

class PlayerController : public game::Component {
    game::PhysicsBehavour* behavour;
    char* resultmsg    = nullptr; 
    char* resoursesmsg = nullptr;
    char* scoremsg     = nullptr;
    char* velocitymsg  = nullptr;

    bool done = false;

    float score  = 8000;
    float fuel = 100;
public:
    PlayerController (
        game::GameObject* gameObject           , 
        game::UITextRenderer* resultrenderer   , 
        game::UITextRenderer* resourserenderer ,
        game::UITextRenderer* scorerenderer    ,
        game::UITextRenderer* velocityrenderer
    ) : behavour(&gameObject->physicsBehavour) , 
        resultmsg(resultrenderer->content)     , 
        resoursesmsg(resourserenderer->content),
        scoremsg(scorerenderer->content)       ,
        velocitymsg(velocityrenderer->content)
    {}

    void update (float deltatime) {
        auto velocity = behavour->getVelocity().magnitude();
        sprintf_s(velocitymsg , 128, "v(mps): %.2f"  , velocity);
        sprintf_s(scoremsg    , 128, "score : %.0f"  , score);
        sprintf_s(resoursesmsg, 128, "fuel  : %.1f%%", fuel);

        if (!done) {
            static float groundt = 0;

            if (behavour->isTriggered()) {
                if (velocity > 3.0f) {
                    sprintf_s(resultmsg, 128, "Mission failed!");

                    done = true;
                    return;
                } else if (groundt >= 0.1f) {
                    sprintf_s(resultmsg, 128, "Mission complete with score %.0f", score);
                    behavour->setActive(false);

                    done = true;
                    return;
                } else {
                    groundt += deltatime;
                }
            } else {
                groundt -= deltatime / 100;
                groundt = std::max(0.0f, groundt);
            }

            score -= 100 * deltatime;

            auto transform = behavour->getTransform();
            if (is_key_pressed(VK_UP)) {
                auto deltafuel = deltatime * 5;

                if (fuel > deltafuel) {
                    fuel -= deltafuel;
                    behavour->force(
                        transform->position,
                        transform->rotation * geom::Vector2f(0.0f, 1.0f) * 52,
                        deltatime
                    );
                }
            }

            if (is_key_pressed(VK_LEFT) || is_key_pressed(VK_RIGHT)) {
                auto deltafuel = deltatime * (int(is_key_pressed(VK_LEFT)) + int(is_key_pressed(VK_RIGHT)));

                if (fuel > deltafuel) {
                    fuel -= deltafuel;
                    behavour->force(
                        transform->position + transform->rotation * geom::Vector2f(0.0f, 1.0f),
                        transform->rotation * (
                            int(is_key_pressed(VK_LEFT))  * geom::Vector2f(-1.0f, 0.0f) +
                            int(is_key_pressed(VK_RIGHT)) * geom::Vector2f( 1.0f, 0.0f)
                        ) * 20,
                        deltatime
                    );
                }
            }
        }
    }  
};

