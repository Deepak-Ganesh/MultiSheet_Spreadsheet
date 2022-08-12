#ifndef SERVER_NETWORK_CONTROLLER_CELL_CHANGE_H
#define SERVER_NETWORK_CONTROLLER_CELL_CHANGE_H

#include "Cell.h"

/** Simple type that pairs a cell's name with its previous value.
 */
class CellChange
{
  public:
    /** Default constructor */
    CellChange();

    /** Direct value constructor */
    CellChange(string name, Cell previous_value, bool is_revert);

    /** JSON deserialize constructor */
    CellChange(json const & js);


    string get_name() const;
    Cell get_previous_value() const;
    bool get_is_revert() const;


    /** Serializes the cell as a JSON object.
     * 
     * @return A json object of the form {"name": string, "previous_value": Cell}
     */
    json serialize() const;

  private:
    /** The name of the cell within the spreadsheet.
     */
    string name;

    /** The previous value (and the associated dependencies) of the cell.
     * 
     * TODO: Remove this?
     */
    Cell previous_value;

    bool is_revert;
};

#endif