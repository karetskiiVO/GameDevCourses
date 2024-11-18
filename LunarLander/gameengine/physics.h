#pragma once

#include <geometry.h>

namespace game {

struct Transform {
    geom::Rotation rotation;
    geom::Vector2f position;
};

}