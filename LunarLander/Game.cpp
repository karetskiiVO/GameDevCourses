#include "Engine.h"
#include <cstdlib>
#include <memory>

#include <gameengine.h>
#include "utilities.h"

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

    auto polygon = geom::Polygon({
        {-4.31f,  2.40f},
        {-1.00f,  5.00f},

        { 0.70f,  4.82f},
        { 0.43f,  6.62f},
        { 3.23f,  6.31f},
        { 2.14f,  4.69f},

        { 3.95f,  4.48f},
        { 3.63f,  2.08f},
        {-1.99f,  0.72f},
        {-0.17f, -4.08f},
        {-0.07f, -7.16f},
        {-4.73f, -5.50f},
    });

    auto floorPolygon = geom::Polygon({
        { 100.0f,  10.0f},
        { 100.0f, -10.0f},
        {-100.0f, -10.0f},
        {-100.0f,  10.0f},
    });

    auto apolo13 = createLander(
        game::Transform{
            .position = {0.0f, 120.0f}
        }
    );
    auto gravicy = new GravicyMaker(apolo13);
    apolo13->components.push_back(gravicy);
    auto cameraMover = new CameraMover(&apolo13->transform, &gameEngine->camera.transform);
    apolo13->components.push_back(cameraMover);

    auto resultMsgRenderer = new game::UITextRenderer();
    resultMsgRenderer->position = geom::Vector2i(512, 350);
    resultMsgRenderer->align = game::UITextRenderer::Align::Center;

    auto resourseMsgRenderer = new game::UITextRenderer();
    resourseMsgRenderer->position = geom::Vector2i(800, 72);
    resourseMsgRenderer->align = game::UITextRenderer::Align::Left;

    auto velocityMsgRenderer = new game::UITextRenderer();
    velocityMsgRenderer->position = geom::Vector2i(800, 40);
    velocityMsgRenderer->align =  game::UITextRenderer::Align::Left;

    auto scoreMsgRenderer = new game::UITextRenderer();
    scoreMsgRenderer->position = geom::Vector2i(800, 8);
    scoreMsgRenderer->align =  game::UITextRenderer::Align::Left;
    
    auto controllerGameObject = new game::GameObject {
        game::Transform{},
        new game::MultiRenderer{
            {resultMsgRenderer, resourseMsgRenderer, velocityMsgRenderer, scoreMsgRenderer}
        },
        game::PhysicsBehavour()
    };
    auto playerController = new PlayerController{
        apolo13             ,
        resultMsgRenderer   ,
        resourseMsgRenderer ,
        velocityMsgRenderer ,
        scoreMsgRenderer    ,
    };
    controllerGameObject->components.push_back(playerController);

    auto moon = createPlanet(game::Transform{}, 80);

    auto fpsRenderer = new game::UIFPSRenderer();
    fpsRenderer->position = geom::Vector2i(8, 8);
    auto fpsCounterGameObject = new game::GameObject{
        game::Transform{}, 
        fpsRenderer,
        game::PhysicsBehavour()
    };

    gameEngine->add(apolo13);
    gameEngine->add(moon);
    gameEngine->add(fpsCounterGameObject);
    gameEngine->add(controllerGameObject);
}

// this function is called to update game data,
// dt - time elapsed since the previous update (in seconds)
void act (float dt) {
    gameEngine->update(dt);
    gameEngine->physicsUpdate(dt);

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

