#include "Engine.h"
#include <cstdlib>
#include <memory>

#include <gameengine.h>
#include "test.h"

//
//  You are free to modify this file
//

//  is_key_pressed(int button_vk_code) - check if a key is pressed,
//                                       use keycodes (VK_SPACE, VK_RIGHT, VK_LEFT, VK_UP, VK_DOWN, 'A', 'B')
//
//  get_cursor_x(), get_cursor_y() - get mouse cursor position
//  is_mouse_button_pressed(int button) - check if mouse button is pressed (0 - left button, 1 - right button)
//  clear_buffer() - set all pixels in buffer to 'black'
//  is_window_active() - returns true if window is active
//  schedule_quit_game() - quit game after act()

game::GameEngine* gameEngine = nullptr;

// initialize game data in this function
void initialize () {
    gameEngine = new game::GameEngine(
        game::Camera(
            reinterpret_cast<uint32_t*>(buffer), 
            SCREEN_HEIGHT,
            SCREEN_WIDTH,
            game::Transform()
        )
    );

    auto Polygon1 = geom::Polygon({
        {-2.67f,  3.80f},
        { 4.03f,  2.54f},
        { 5.07f, -0.54f},
        {-0.65f, -4.16f},
        {-6.89f, -1.52f},
    });

    auto Polygon2 = geom::Polygon({
        {-2.67f,  4.00f},
        { 2.11f,  2.28f},
        { 2.13f, -1.04f},
        {-2.07f, -2.40f},
    });

    auto testGameObject1 = new game::GameObject(
        game::Transform{
            .position = {-10.0f, 0}
        }, 
        new game::PolygonRenderer(Polygon1)
    );
    auto testGameObject2 = new game::GameObject(
        game::Transform{
            .position = {10.0f, 0}
        }, 
        new game::PolygonRenderer(Polygon2)
    );

    auto textRenderer = new game::UITextRenderer();
    textRenderer->position = geom::Vector2i(8, 8);
    auto fpsCounterGameObject = new game::GameObject(game::Transform{}, textRenderer);
    auto fpsCounter = new FPScounter(textRenderer->content);

    fpsCounterGameObject->components.push_back(fpsCounter);

    gameEngine->add(testGameObject1);
    gameEngine->add(testGameObject2);
    gameEngine->add(fpsCounterGameObject);
}

// this function is called to update game data,
// dt - time elapsed since the previous update (in seconds)
void act (float dt) {
    gameEngine->update(dt);

    if (is_key_pressed(VK_ESCAPE))
        schedule_quit_game();
}

// fill buffer in this function
// uint32_t buffer[SCREEN_HEIGHT][SCREEN_WIDTH] - is an array of 32-bit colors (8 bits per R, G, B)
void draw () {
    gameEngine->render();
}

// free game data in this function
void finalize () {
}

