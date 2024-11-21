#include <Polygon.h>
#include <Windows.h>
#include <string>


std::vector<geom::Polygon> geom::splitToConvex (const geom::Polygon& poly) {
    std::vector<geom::Polygon> polygons;

    auto rightTupple = [] (const Point& p1, const Point& p2, const Point& p3) {
        return cross(p2 - p1, p3 - p2) >= 0;
    };

    std::deque<Point> points;
    for (auto vert : poly.verticies) points.push_back(vert);

    size_t resetcnt = 0;

    while (true) {
        auto p1 = points.front();
        points.pop_front();
        auto p2 = points.front();
        points.pop_front();
        auto p3 = points.front();
        points.pop_front();

        OutputDebugStringA(("iterations: " + std::to_string(points.size()) + "\n").c_str());

        if (points.size() == 4) {
            polygons.push_back(geom::Polygon{{points.begin(), points.end()}});
            break;
        }

        if (rightTupple(p1, p2, p3)) {
            resetcnt = 0;
            polygons.push_back(geom::Polygon{{p1, p2, p3}});
            if (!points.size()) break;

            points.push_back(p1);
            points.push_back(p3);
        } else {
            resetcnt++;
            points.push_back(p1);

            points.push_front(p3);
            points.push_front(p2);
        }

        if (resetcnt == 10000) {
            break;
        } 
    }
    
    return polygons;
}