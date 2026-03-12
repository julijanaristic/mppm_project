# Smart Grid Data Model - CIM Based Project
This project was developed as part of the Data Models in Smart Grids course.

The goal of the project is to design and implement a CIM-based data model representing elements of an electrical power network and to integrate the model with the Network Management System (NMS) using the Generic Data Access (GDA) interface.

The implementation includes:
- Creating a CIM profile
- Generating a DLL from the model
- Implementing ModelCodes
- Implementing GDA methods
- Creating a GUI application for reading data from the system

## Project Workflow
The project was developed following the standard workflow used in CIM-based systems.

### Analyze UML Diagram 
The starting point of the project was a UML model stored in an Enterprise Architect (EAP) file. The file can be opened using: Enterprise Architect Lite / Viewer

Steps:
- Open the .EAP file using Enterprise Architect Viewer (32-bit)
- Navigate to IES_Projects
- Find the assigned project diagram
- Study the diagram carefully before implementation

Notes:
- The EA viewer is used only for viewing diagrams
- The label {isSubstitutable=false} can be ignored because it usually indicates inheritance

## CIM Model Structure
The data model consists of several classes organized through inheritance and references.

### Base Class
#### IdentifiedObject (abstract)
Attributes:
- aliasName
- mRID
- name
Inherited by:
- PowerSystemResource
- Terminal
- ConnectivityNode

### Power System Hierarchy
#### PowerSystemResource (abstract)
No attributes
Inherited by:
- Equipment

#### Equipment (abstract)
No attributes
Inherited by:
- ConductingEquipment

#### ConductingEquipment (abstract)
No attributes
Inherited by:
- Conductor
- RectifierInverter
- Clamp
Relationships:
- Can reference 0..* Terminal objects

### Equipment Classes
#### RectifierInverter (concrete)
No attributes

#### Conductor (abstract)
No attributes
Inherited by:
- ACLineSegment

#### ACLineSegment (concrete)
No attributes
Relationships:
- Can reference 0..* Clamp objects


#### Clamp (concrete)
Attributes:
- lengthFromTerminal
Relationships:
- Must reference exactly one ACLineSegment object

### Network Connectivity Classes
#### Terminal (concrete)
Attributes:
- connected
- phases
- sequenceNumber
Relationships:
- Must reference exactly one ConductingEquipment object
- May reference 0..1 ConnectivityNode objects

#### ConnectivityNode (concrete)
No attributes
Relationships:
- Can reference 0..* Terminal objects

## CIM Profile Creation
The CIM profile was created according to the UML diagram.
The following rules were applied:
- All classes, attributes, and enumerations were transferred to the profile
- Abstract and concrete classes were preserved
- Relationship cardinalities were respected
- Only relationships where at least one side had max cardinality 1 were described
- If both sides had *, one side was treated as having max cardinality 1
- Complex types such as ElectronicAddress were removed
- Enumerations were preserved
- All relationships were marked as ByReference

After creating the profile:
- The profile was exported in legacy-rdfs format
- A DLL was generated

## Code Generation and Adapter
The generated DLL was then:
- Copied into the Adapter project
- Referenced inside the Adapter
- Used to implement mapping methods

Additionally:
- ModelCodes were defined
- Mapping logic between CIM model and NMS objects was implemented

## GDA Implementation
The system supports Generic Data Access (GDA) operations.

The following methods were implemented:
- GetProperty
- SetProperty
- AddReference
- RemoveReference
- GetReferences

These methods allow interaction with objects stored in the Network Management System (NMS).

## Data Import
Concrete instances of objects were created using CIMET.

Steps:
- Load the CIM profile
- Generate RDF snippets
- Create RdfSnippet.xml with sample data
The dataset contains:
- At least two instances of each concrete class (in this project there are four instances)

## Loading Data into the System
To use the application, the data must first be imported into the system.

Steps:
- Open ModelLabs application
- Load RdfSnippet.xml
- Convert the file
- Execute Apply Delta

After this step:
- Objects are created in the Network Management System
- Each object receives a GID (Global Identifier)

These GIDs are required for querying objects in the GUI application.

## GUI Application
A GUI application was implemented to allow users to interact with the system using GDA methods.

The application contains several tabs.

### GetValues Tab
Allows retrieving all properties of a specific object.

Input:
- GID

Output:
- List of all properties of that object

### GetExtentValues Tab
Allows retrieving all objects of a specific concrete class.

Supported classes:
- Terminal
- ACLineSegment
- Clamp
- RectifierInverter
- ConnectivityNode

Output:
- List of objects
- All their properties

### GetRelatedValues Tab
Allows retrieving objects connected through references.

Input:
- Object GID
- Reference type

Output:
- Related objects

### ACLineSegment and Clamp Tab
This tab focuses on the relationship between ACLineSegment and Clamp objects.

Features:
- Enter the GID of an ACLineSegment
- Retrieve all clamps connected to that ACLineSegment

Additional feature:
- Button that finds the clamp with the smallest length

### Terminal Tab
This tab provides operations related to Terminal objects.

Available options:
- Retrieve all terminals
- Retrieve all connected terminals
- Retrieve all disconnected terminals

## Conclusion
The project demonstrates the implementation of a CIM-based data model for smart grid systems integrated with the Network Management System.

Key achievements include:
- Creation of a structured CIM profile
- Implementation of ModelCodes and GDA methods
- Integration of generated model classes into the Adapter
- Successful data import and system interaction
- Development of a GUI application for data querying

The system provides a clear example of how CIM data models can be integrated into smart grid infrastructure, enabling efficient data management, system monitoring, and future scalability.
