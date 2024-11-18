# CMakeList.txt: проект CMake для LunarLander; включите исходный код и определения,
# укажите здесь логику для конкретного проекта.
#
cmake_minimum_required (VERSION 3.8)

project ("LunarLander")

include_directories(geometry gameengine test)

# Добавьте источник в исполняемый файл этого проекта.
add_executable (LunarLander WIN32 "Game.cpp" "Engine.cpp" "geometry/rotation.cpp")

if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET LunarLander PROPERTY CXX_STANDARD 20)
endif()

# TODO: Добавьте тесты и целевые объекты, если это необходимо.
