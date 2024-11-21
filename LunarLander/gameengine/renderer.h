#pragma once

#include <font.h>
#include <camera.h>
#include <geometry.h>

#include <chrono>

namespace game {

struct Renderer {
    virtual void render (Camera& camera, const Transform& gameObjectTransform) {}
};

struct SegmentRenderer : public Renderer {
    geom::Vector2f begin, end;

    SegmentRenderer (geom::Vector2f begin, geom::Vector2f end) : begin(begin), end(end) {}

    void render (Camera& camera, const Transform& gameObjectTransform) {
        camera.drawSegment(
            gameObjectTransform.position + begin,
            gameObjectTransform.position + end,
            RGBColor(255, 255, 255)
        );
    }
};

struct PolygonRenderer : public Renderer {
    geom::Polygon polygon;

    PolygonRenderer (const geom::Polygon& polygon) : polygon(polygon) {}

    void render (Camera& camera, const Transform& gameObjectTransform) {
        for (size_t i = 0; i < polygon.verticies.size() - 1; i++) {
            camera.drawSegment(
                gameObjectTransform.position + gameObjectTransform.rotation * polygon.verticies[i],
                gameObjectTransform.position + gameObjectTransform.rotation * polygon.verticies[i + 1],
                RGBColor(255, 255, 255)
            );
        }

        camera.drawSegment(
            gameObjectTransform.position + gameObjectTransform.rotation * polygon.verticies.back(),
            gameObjectTransform.position + gameObjectTransform.rotation * polygon.verticies[0],
            RGBColor(255, 255, 255)
        );
    }
};

struct UITextRenderer : public Renderer {
    geom::Vector2i position;
    char content[128];
    int scale = 2;

    void render (Camera& camera, const Transform&) {
        auto currPosition = position;

        auto ptr = content;
        while (*ptr != 0) {
            camera.drawUICharacter(currPosition, font[*ptr], RGBColor(255, 255, 255), scale);
            currPosition.x += 8*scale;
            ptr++;
        }
    }
};

struct UIFPSRenderer : public UITextRenderer {
    void render (Camera& camera, const Transform& transform) {
        static auto prev = std::chrono::steady_clock::now();
        auto now = std::chrono::steady_clock::now();
        const std::chrono::duration<double> diff{now - prev};

        sprintf_s(content, 128, "fps: %.2f", 1e6f / std::chrono::duration_cast<std::chrono::microseconds>(diff).count());
        prev = now;
        UITextRenderer::render(camera, transform);
    }
};


struct MultiRenderer : public Renderer {
    std::vector<Renderer*> renderers;

    MultiRenderer (const std::vector<Renderer*>& renderers = {}) : renderers(renderers) {}

    void render (Camera& camera, const Transform& gameObjectTransform) {
        for (auto rendererptr : renderers) rendererptr->render(camera, gameObjectTransform);
    }

    MultiRenderer* addPolygons (const std::vector<geom::Polygon>& polygons) {
        for (const auto& polygon : polygons) renderers.push_back(new PolygonRenderer(polygon));
        return this;
    }
};

}