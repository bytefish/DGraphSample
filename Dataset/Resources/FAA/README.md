# Preprocessing the FAA Aircraft Registry Database #

The FAA [Aircraft Registry Releasable Aircraft Database] contains registered aircrafts and associtated 
data, such as tail number, seats, engine types. 

The dataset is available at:

* [https://www.faa.gov/licenses_certificates/aircraft_certification/aircraft_registry/releasable_aircraft_download/](https://www.faa.gov/licenses_certificates/aircraft_certification/aircraft_registry/releasable_aircraft_download/)

It is described as:

> The archive file contains the following files:
> 
> * Aircraft Registration Master file
> * Aircraft Dealer Applicant file
> * Aircraft Document Index file
> * Aircraft Reference file by Make/Model/Series Sequence
> * Deregistered Aircraft file
> * Engine Reference file
> * Reserve N-Number file
> 
> Files are updated each federal business day. The records in each database file are stored in a comma delimited format (CDF) 
> and can be manipulated by common database management applications, such as MS Access.

## Processing the data with Excel and Power Query ##

Basically the original FAA dataset contains 6 CSV files:

* ``ACTREF.txt``
* ``DEALER.txt``
* ``DEREG.txt``
* ``DOCINDEX.txt``
* ``ENGINE.txt``
* ``MASTER.txt``

The files reference each other by unqiue identifiers, so it's possible to join the data. 

[Power Query] makes it easy to transform the data. Microsoft writes on Power Query:

> Power Query is the Data Connectivity and Data Preparation technology that enables end users to 
> seamlessly import and reshape data from within a wide range of Microsoft products, including Excel, 
> Power BI, Analysis Services, Common Data Services for Apps & Analytics, and more.

I provide an Excel file, that contains all Power Queries to produce the merged CSV file:

* [https://github.com/bytefish/ApacheJenaSample/blob/master/Dataset/Resources/FAA/FAA_AircraftRegistration_Database.xlsx](https://github.com/bytefish/ApacheJenaSample/blob/master/Dataset/Resources/FAA/FAA_AircraftRegistration_Database.xlsx)

I basically imported the CSV Files and put them in a group called ``Raw Data``. Then I performed joins between 
the tables and put the results in a group called ``Merged Data``. The location of the raw CSV files is given by 
the parameters in the group ``Parameters``:

<a href="https://raw.githubusercontent.com/bytefish/ApacheJenaSample/master/Dataset/Resources/FAA/Images/PowerQueryEditor_Overview.png">
	<img src="https://raw.githubusercontent.com/bytefish/ApacheJenaSample/master/Dataset/Resources/FAA/Images/PowerQueryEditor_Overview.png" width="30%" height="30%" alt="Power Query Editor Groups" />
</a>

To read the data, you have to adjust the path to the CSV Files:

<a href="https://raw.githubusercontent.com/bytefish/ApacheJenaSample/master/Dataset/Resources/FAA/Images/PowerQueryEditor_Parameters.png">
	<img src="https://raw.githubusercontent.com/bytefish/ApacheJenaSample/master/Dataset/Resources/FAA/Images/PowerQueryEditor_Parameters.png" width="50%" height="50%" alt="Power Query Editor Groups" />
</a>

The final dataset is a CSV export of the ``MergedData_Reduced`` Excel Spreadsheet.

[Power Query]: https://docs.microsoft.com/en-us/power-query/
[Aircraft Registry Releasable Aircraft Database]: https://www.faa.gov/licenses_certificates/aircraft_certification/aircraft_registry/releasable_aircraft_download/
