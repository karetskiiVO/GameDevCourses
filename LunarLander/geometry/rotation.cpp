#include "rotation.h"

#include <cmath>

using namespace geom;

Rotation::Rotation (float angle) : cos(std::cos(angle)), sin(std::sin(angle)) {} 
Rotation::Rotation (Vector2f dir) {
    auto mag = dir.magnitude();
    cos = dir.x / mag;
    sin = dir.y / mag;
}
Rotation::Rotation (float cos, float sin) : cos(cos), sin(sin) {}

Vector2f Rotation::direction () const {
    return Vector2f{cos, sin};
}

Rotation Rotation::operator+ (Rotation r) const {
    return Rotation{cos * r.cos - sin * r.sin, sin * r.cos + cos * r.sin};
}
Rotation Rotation::operator- (Rotation r) const {
    return Rotation{cos * r.cos + sin * r.sin, sin * r.cos - cos * r.sin};
}
Rotation Rotation::operator- () const {
    return Rotation(0) - *this;
}
Vector2f Rotation::operator* (Vector2f vect) const {
    return Vector2f{cos * vect.x - sin * vect.y, sin * vect.x + cos * vect.y};
}

Rotation& Rotation::operator+= (Rotation r) {
    *this = *this + r;
    return *this;
}
Rotation& Rotation::operator-= (Rotation r) {
    *this = *this - r;
    return *this;
}