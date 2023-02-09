﻿namespace SchuetzenOne;

public static class Constants
{
    public const string DatabaseFilename = "schuetzenOne.db";

    public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath => 
        Path.Combine("D:\\Users\\rL33T\\Documents\\VS 2022\\SchuetzenOne", DatabaseFilename);
}