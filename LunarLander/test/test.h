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
        sprintf_s(content, 128, "distance: %.2f", distance);
    }
};

class GravicyMaker : public game::Component {
    game::PhysicsBehavour* behavour;

public:
    GravicyMaker (game::GameObject* gameObject) : behavour(&gameObject->physicsBehavour) {}

    void update (float deltatime) {
        behavour->force(behavour->getTransform()->position, geom::Vector2f{0, -40.0}, deltatime);
    }
};


