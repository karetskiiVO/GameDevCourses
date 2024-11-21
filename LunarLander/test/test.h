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