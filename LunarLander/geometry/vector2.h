#pragma once

#include <cmath>

namespace geom {

template <typename T>
struct Vector2 {
    T x, y;

    Vector2 (const T x, const T y) : x(x), y(y) {}
    Vector2 () : x(0), y(0) {}

    template <typename ScalarT>
    auto operator* (ScalarT scalar) const -> Vector2<decltype(x * scalar)> {
        return {x * scalar, y * scalar};
    }
    template <typename ScalarT>
    auto operator/ (ScalarT scalar) const -> Vector2<decltype(x / scalar)> {
        return {x / scalar, y / scalar};
    }

    Vector2 operator+ (Vector2 vector) const {
        return {x + vector.x, y + vector.y};
    }
    Vector2 operator- (Vector2 vector) const {
        return {x - vector.x, y - vector.y};
    }

    Vector2& operator+= (Vector2 vector) {
        *this = *this + vector;
        return *this;
    }
    Vector2& operator-= (Vector2 vector) {
        *this = *this - vector;
        return *this;
    }

    auto magnitude2 () const -> decltype(x*x + y*y) {
        return x*x + y*y;
    }

    auto magnitude () const -> decltype(x*x + y*y) {
        return std::abs(magnitude2());
    }

    // static const Vector2 zero;
    // static const Vector2 up;
    // static const Vector2 left;
    // static const Vector2 down; 
    // static const Vector2 right;
};

template <typename T, typename ScalarT>
auto operator* (ScalarT scalar, Vector2<T> v) -> decltype(v * scalar) {
    return v * scalar;
}

template <typename T>
auto dot (Vector2<T>& v1, Vector2<T>& v2) -> decltype(v1.x * v2.x + v1.y * v2.y) {
    return v1.x * v2.x + v1.y * v2.y;
}

using Vector2f = Vector2<float>;
using Vector2i = Vector2<int>;
using Vector2d = Vector2<double>;

using Point = Vector2f;

}