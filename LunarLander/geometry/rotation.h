#pragma once

#include "Vector2.h"

namespace geom {

class Rotation {
    float cos, sin;

    Rotation (float cos, float sin);
public:
    Rotation (float radAngle = 0);
    Rotation (Vector2f dir);

    Rotation operator+ (Rotation r) const;
    Rotation operator- (Rotation r) const;
    Vector2f operator* (Vector2f vect) const;

    Vector2f direction() const;
};

}