/** Contains function definitions for Server.
 *
 *  16 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#include "UserAccountCollection.h"

#include <iostream>

/** Adds the account if it doesn't exist, returning true.
 * 
 * @param username The username of the account.
 * @param password The password of the account.
 * 
 * @return True if the username didn't previously exist.
 */
bool UserAccountCollection::add_account(std::string username, std::string password)
{
  std::lock_guard<std::mutex> lg {mut};
  
  if (nameToPasswordMap.find(username) == nameToPasswordMap.end())
  {
    nameToPasswordMap.emplace(username, password);
    return true;
  }
  else
  {
    return false;
  }
}

/** Remove the account if it exists, returning true.
 * 
 * @param username The username of the account.
 * 
 * @return True if the username previously existed.
 */
bool UserAccountCollection::delete_account(std::string username)
{
  std::lock_guard<std::mutex> lg {mut};
  
  if (nameToPasswordMap.find(username) == nameToPasswordMap.end())
  {
    return false;
  }
  else
  {
    map<string, string>::iterator it = nameToPasswordMap.find(username);
    nameToPasswordMap.erase(it);
    return false;
  }
}

/** Change the password if account exists, returning true.
 * 
 * @param username The username of the account.
 * @param password The password of the account.
 * 
 * @return True if the username exists.
 */
bool UserAccountCollection::change_password(std::string username, std::string password)
{
  std::lock_guard<std::mutex> lg {mut};

  std::map<string, string>::iterator it = nameToPasswordMap.find(username);

  if (it != nameToPasswordMap.end())
  {
    it->second = password;
    return true;
  }
  return false;
}


/** Adds the account if it doesn't exist, or checks the password
 * if it does exist.
 * 
 * @param username The username of the account to authenticate.
 * @param password The password of the account to authenticate.
 * 
 * @return True if the username exists and the password matches
 */
bool UserAccountCollection::authenticate(std::string username, std::string password)
{
  std::lock_guard<std::mutex> lg {mut};

  auto it = nameToPasswordMap.find(username);
  if (it == nameToPasswordMap.end())
  {
    return false;
  }
  else
  {
    if (it->second == password)
      return true;
    else
      return false;
  }
}



/** Authenticates the user if it exists, otherwise creates it.
 * 
 * @param username The username of the account.
 * @param password The passwor of the account.
 * 
 * @return True if the username and password match, or if the user didn't
 *         previously exist. False if the username and password don't match.
 */
bool UserAccountCollection::attempt_authentication(std::string username, std::string password)
{
  std::lock_guard<std::mutex> lg {mut};

  auto it = nameToPasswordMap.find(username);
  if (it == nameToPasswordMap.end())
  {
    nameToPasswordMap.emplace(username, password);
    return true;
  }
  else
  {
    if (it->second == password)
      return true;
    else
      return false;
  }
}



/** Creates a json object from this object's state.
 * The json object is a dictionary of usernames mapped to passwords.
 * 
 * @return The json object.
 */
nlohmann::json UserAccountCollection::serialize()
{
  // TODO: Ensure no dead lock
  std::lock_guard<std::mutex> lg (mut);
  json js = json::object_t();

  for (auto & i : nameToPasswordMap)
  {
    js[i.first] = i.second;
  }

  return js;
}


nlohmann::json UserAccountCollection::serialize_names()
{
  std::lock_guard<std::mutex> lg (mut);
  json js = json::array_t();

  for (auto & i : nameToPasswordMap)
  {
    js.push_back(i.first);
  }

  return js;
}

std::vector<std::string> UserAccountCollection::get_names()
{
  std::lock_guard<std::mutex> lg (mut);
  std::vector<std::string> names;

  for (auto & i : nameToPasswordMap)
  {
    names.push_back(i.first);
  }

  return names;
}


/** Sets the internal data to the json-serialized data.
 * The json object must be a dictionary of usernames mapped to passwords.
 * 
 * @param js The json object to deserialize.
 */
void UserAccountCollection::deserialize(nlohmann::json & js)
{
  std::lock_guard<std::mutex> lg (mut);
  nameToPasswordMap = js.get<std::map<std::string, std::string>>();
}
