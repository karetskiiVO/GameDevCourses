#pragma once

#include <gameengine.h>

#include <cstdio>

class FPScounter : public game::Component {
    char* content;

public:
    FPScounter (char* output) : content(output) {}

    void update (float deltatime) {
        sprintf_s(content, 128, "fps:%.2f", 1/deltatime);
    }
};

class DebugWriter : public game::Component {
    char* content;

public:
    DebugWriter (char* output) : content(output) {}

    void update (float deltatime) {
        //sprintf_s(content, 128, "%s", debugLog.c_str());
    }
};

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

struct CameraMover : public game::Component {
    game::Transform* objectTransform = nullptr;
    game::Transform* cameraTransform = nullptr;

    CameraMover (game::Transform* objectTransform, game::Transform* cameraTransform) 
        : objectTransform(objectTransform), cameraTransform(cameraTransform) {}
    
    void update (float) {
        cameraTransform->position = objectTransform->position;
    }
};


