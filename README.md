# 0x2eNEET 
Common code and various extensions to Microsoft .NET Framework.
# Status
Partly not functionnal (some untested changes were made)
# What is that ?
See when you have to maintain multiple projects and they ALL have some parts of code in common ?
Wrappers, small utilities and tools ? 

Well this is somewhat a lib to merge them all, avoiding the "stackoverflowing/googling for the forgotten piece of code you found that day that worked".

# Main parts 
* Core
    * Common : Classes reusable in various cases (generic or not)
    * Extensions : Extensions classes to general classes (may they be in this lib or in the .NET)
    * DebugTools : Various utilities used for debugging purposes
    * Functions : General purpose functions and algorithms, they may have their extensions too
*  WPF
    * ViewModels : Generic classes used for viewmodels, oftenly binding related
    * UserControls
* Xml
    * Common : Classes, shortcuts, wrappers used for Xml exclusively
    * DataAccess : Classes used to have a simplified access or custom behaviour to query/manipulate data

As you may have noticed, each main part has his own project, and each sub part has his own folder.

# Points of interests
  
As there isn't a doc (yet), here few classes that might catch your interest (non exhaustive) :
  * Executer : 
    * Implement a producer/consumer running in a single and own thread. 
    * Likely used to implement a thread safety around an API or anything else.
    * Has both normal and async method calls
    * Normal calls wait for the delegate's end to return
  * DiffContextAccess :
    * Insert/Update/Delete operation based Xml data contexts
    * May be used for JSon or other simple serializer
    * Not an ORM (!)
    * Parent/children (with n depth) relation ship between datacontext
    * Children level changes on data inherited from parent is saved as a diff
    * Change made to parents impact children

The features overall are fairly simplistic : it's not your big lib that does everything. 
But it's done with the hope it'll do just the job, nothing more and are easily understandable and debuggable by anyone.

# Todo

 * Integrate the rest of my common codebase (All the WPF stuff and other lost piece of code)
 * Units tests (import & create)
 * Documentation (javadoc style)
 * Samples
