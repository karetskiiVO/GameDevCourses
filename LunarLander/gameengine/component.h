#pragma once

#include <vector>

namespace game {

struct Component {
    virtual void awake   () {}
    virtual void start   () {}
    virtual void destroy () {}

    virtual void update (float deltatime) {}
};

template <typename ComponentT>
class ComponentCreator {
    std::vector<ComponentT*> allocated;
public:
    ComponentCreator () = default;

    ComponentCreator (const ComponentCreator&)              = delete;
    ComponentCreator (ComponentCreator&&)                   = default;
    ComponentCreator& operator= (const ComponentCreator&)   = delete;
    ComponentCreator& operator= (ComponentCreator&&)        = default;

    ~ComponentCreator() {
        for (auto ptr : allocated) delete ptr;
    }

    // template<typename ...Arguments>
    // ComponentT* Create (Arguments... args) {
    //     auto res = new T(args...);
    //     allocated.push_back(res);
    //     return res;
    // }
};

}