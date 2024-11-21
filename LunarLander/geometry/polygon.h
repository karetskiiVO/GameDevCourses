#pragma once

#include <vector>
#include <algorithm>
#include <limits>
#include <numbers>
#include <functional>
#include <map>
#include <queue>

#include <rotation.h>
#include <vector2.h>

namespace geom {

struct Polygon {
    std::vector<Vector2f> verticies;

    Polygon (const std::vector<Vector2f>& verticies) : verticies(verticies) {}
};

std::vector<Polygon> splitToConvex (const Polygon& poly);

}