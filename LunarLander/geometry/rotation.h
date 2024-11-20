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
    Rotation operator- () const;
    Vector2f operator* (Vector2f vect) const;

    Rotation& operator+= (Rotation r);
    Rotation& operator-= (Rotation r);

    Vector2f direction() const;
};

}