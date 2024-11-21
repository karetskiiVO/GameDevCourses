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
    float scale = 1;
    Transform transform;

    Camera (uint32_t* screen, int height, int width, Transform transform = Transform()) :
        screen(screen), height(height), width(width), transform(transform) {} // except < 0

    void clear () {
        memset(screen, 0, height * width * sizeof(uint32_t));
    }

    void drawPixel (int x, int y, RGBColor color) {
        auto screenx = x + width/2;
        auto screeny = height/2 - y;

        if (0 <= screenx && screenx < width &&
            0 <= screeny && screeny < height)  screen[screeny * width + screenx] = color.col;
    }

    void drawCircle (geom::Point coord, RGBColor color, float radius) {
        auto localCoord = (-transform.rotation) * (coord - transform.position);

        // in field
        int radiusInt  = std::max(1, static_cast<int>(std::round(radius * scale)));
        int xCenter = static_cast<int>(std::round(localCoord.x * scale));
        int yCenter = static_cast<int>(std::round(localCoord.y * scale));

        for (int y = -radiusInt; y < radiusInt; y++) {
            for (int x = -radiusInt; x < radiusInt; x++) {
                if (x*x + y*y > radiusInt*radiusInt) continue;

                drawPixel(x + xCenter, y + yCenter, color);
            }
        }
    }

    void drawSegment (geom::Vector2f start, geom::Vector2f end, RGBColor color) {
        auto localStart = (-transform.rotation) * (start - transform.position);
        auto localEnd   = (-transform.rotation) * (  end - transform.position);

        auto xStart = static_cast<int>(std::round(localStart.x * scale));
        auto yStart = static_cast<int>(std::round(localStart.y * scale));
        auto xEnd   = static_cast<int>(std::round(  localEnd.x * scale));
        auto yEnd   = static_cast<int>(std::round(  localEnd.y * scale));

        int xerr = 0, yerr = 0; 
        int dx = xEnd - xStart;
        int dy = yEnd - yStart;

        int incX = (dx == 0) ? 0 : (dx / std::abs(dx));
        int incY = (dy == 0) ? 0 : (dy / std::abs(dy));

        dx = std::abs(dx);
        dy = std::abs(dy);

        int d = std::max(dx, dy);

        int x = xStart;
        int y = yStart;

        for (int i = 0; i <= d; i++) {
            xerr += dx;
            yerr += dy;

            if (xerr >= d) {
                xerr -= d;
                x += incX;
            }    
            if (yerr >= d) {
                yerr -= d;
                y += incY;
            }

            drawPixel(x, y, color);
        }
    }

    // draws 8*16 letter
    void drawUICharacter (geom::Vector2i position, const uint8_t* letter, RGBColor color, int scale) {
        for (uint16_t y = 0; y < 16u; y++) {
            for (uint16_t x = 0; x < 8u; x++) {
                if (!(letter[y] & (uint16_t(1) << x))) continue;

                for (int dy = y * scale; dy < (y+1)*scale; dy++) {
                    for (int dx = x*scale; dx < (x+1)*scale; dx++) {
                        auto x_ = 8*scale - dx + position.x;
                        auto y_ = dy + position.y;
                        
                        if (0 <= x_ && x_ < width && 0 <= y_ && y_ < height) 
                                screen[y_ * width + x_] = color.col;
                    }
                }
            }
        }
    }
};

}