# Setup

Install a postgres database.

# Configuration

In your project use App.settings for the Database connection. 

`
<?xml version="1.0" encoding="utf-8" ?>
<configuration>

	<appSettings>
		<add key="host" value="localhost"/>
		<add key="username" value="postgres"/>
		<add key="database" value="postgres"/>
		<add key="password" value="postgres"/>
		<add key="port" value="5433"/>
	</appSettings>
</configuration>
`

# Building the Datamodel
1. Create a repository that inherits from ORM.Repository.Context.\n
3. Create DBSets for the Objects you want to persists in your app. \n
  Objects are persisted based on type so there is no need to add every nested object.
3. Use Annotations if needed. 
  TableName => Custom table name. 
  ColumnName => Custom column name. 
  PrimaryKey => Primary Key (autoincrement by default).
  CascadeDelete => Points to the field that should be deleted if this class gets deleted (granted that field is a dependency).
  Unique => Unique Key.
4. Use Repository.build() to build the datamodel/to save Database model into the memory.
# Inserting
1. Fill DBSet with data. 
2. Use DBSet Insert(). 
# Querying
1. Create Expression for the query. 
2. Use DBSet.get().
# Deleting 
1. Create Expression for the query. 
<<<<<<< HEAD
2. Use DBSet.delete(). 
=======
2. Use DBSet.delete(). 
>>>>>>> e6ce4cb35068eac980f03bd6bff81b138bab674c
