# Dynamic Data Visualization System

This system dynamically generates various types of graphs from different data sets. For example, it can visualize:

- **Bank Accounts**: Nodes represent bank accounts, and edges represent transactions between these accounts.
- **Social Networks**: Nodes represent users, and edges represent relationships or interactions between them.
- **Project Management**: Nodes can be tasks or milestones, with edges showing dependencies or progress.

## Key Features

- **CRUD Operations**: Create, Read, Update, and Delete roles, users, graphs, files, and categories.
- **File Management**: Add and remove files, and manage file access for specific users.
- **Password Recovery**: Recover passwords via email.
- **Cloud Image Upload**: Upload images using cloud services.
- **High-Performance File Upload**: Add files with 5000 records with 90000 value of edges in 8 seconds.
- **Data Analysis**: Analyze and visualize complex data sets.

## Database Structure

The database is designed using an Entity-Attribute-Value (EAV) model. This structure allows flexible and scalable data storage. Here's a simplified view of the database schema:

![Database Structure](https://github.com/mahdijafariii/Data-analyze-Code_star/blob/main/resource/Untitled.png)


## Demo Video
[![Watch the video](https://img.youtube.com/vi/yxnufdDJ-ek/maxresdefault.jpg)](https://www.youtube.com/watch?v=yxnufdDJ-ek)


## Installation and Setup Steps

### 1. Install Docker
To run this project, you need to have Docker installed:
- **Docker**: [Download and Install Docker](https://www.docker.com/get-started)

First, download and install Docker from the link below:

- [Docker Installation Guide](https://www.docker.com/get-started)

### 2. Clone the Repository

Clone the project repository:

```bash
git clone https://github.com/YourUsername/Summer1403-Project-Group03.git
cd Summer1403-Project-Group03
docker-compose up -d
```

### 3. Start Service
```bash
docker-compose up -d
```





