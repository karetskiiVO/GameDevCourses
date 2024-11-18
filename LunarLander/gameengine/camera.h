#pragma once

#include <physics.h>

#include <cmath>
#include <climits>
#include <memory>
#include <cstdint>
#include <algorithm>

#include <iostream>

namespace game {

class RGBColor {
    uint32_t col;
public:
    RGBColor (uint8_t red, uint8_t green, uint8_t blue) {
        col = uint32_t(red) | (uint32_t(green) << 8) | (uint32_t(green) << 16);
    }

    friend class Camera;
};

class Camera {
    uint32_t* screen = nullptr;
    int height = 0;
    int width  = 0;
public:
    float scale = 10;
    Transform transform;

    Camera (uint32_t* screen, int height, int width, Transform transform = Transform()) :
        screen(screen), height(height), width(width), transform(transform) {} // except < 0

    void clear () {
        memset(screen, 0, height * width * sizeof(uint32_t));
    }

    void drawPoint   (geom::Point coord, RGBColor color, float radius) {
        auto localCoord = coord - transform.position;

        // in field
        int radiusInt  = std::max(1, static_cast<int>(std::round(radius * scale)));
        int xCenter = static_cast<int>(std::round(localCoord.x * scale));
        int yCenter = static_cast<int>(std::round(localCoord.y * scale));

        for (int y = -radiusInt; y < radiusInt; y++) {
            for (int x = -radiusInt; x < radiusInt; x++) {
                if (x*x + y*y > radiusInt*radiusInt) continue;

                auto screenx = width/2  - (x + xCenter);
                auto screeny = height/2 - (y + yCenter);

                if (0 <= screenx && screenx < width &&
                    0 <= screeny && screeny < height)  screen[screeny * width + screenx] = color.col;
            }
        }
    }
    void drawSegment (geom::Vector2f coord, RGBColor col, uint8_t thik = 3) {}
};

}