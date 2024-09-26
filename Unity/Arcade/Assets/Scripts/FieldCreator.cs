using System.Collections.Generic;

namespace Life {

public interface IFieldCreator {
    Tile[] CreateField (Field field);
}

}