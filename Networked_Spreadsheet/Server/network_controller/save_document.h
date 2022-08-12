#ifndef SERVER_SAVE_DOCUMENT_H
#define SERVER_SAVE_DOCUMENT_H

#include <fstream>

#include "Document.h"


// TODO: implement
/** Saves all of a document's data to a file.
 */
std::string serialize_document(Document const & document);


#endif