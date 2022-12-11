using System;
using System.Collections.Generic;

public class Directory {
  IDictionary<string, List<string>> directoriesAndChildren = new Dictionary<string, List<string>>();
  List<string> directories;
  IDictionary<string, int> directorySizes = new Dictionary<string, int>();
  
  public Directory(string[] commands) {
    List<string> currentDirectory = new List<string>();
    directories = new List<string>();

    //directorySizes.Add("root", -1);
    
    foreach(string line in commands) {
      // Commands
      if (line[0] == '$') {
        // cd
        if (line[2] == 'c') {
          if (line[5] == '.'){
            // cd ..
            // Backtrack to higher up
            currentDirectory.RemoveAt(currentDirectory.Count - 1);
          } else {
            // cd b 
            // change directory
            // Add directory to list. Descend.
            currentDirectory.Add(line.Substring(5, line.Length-5));
          } 
        }
      } else {
        string name = "";
        int size = -1;
        if (line[0] == 'd') {  
          //   dir d 
          // make a directory
          name = line.Substring(4, line.Length-4);
        } else {
          // make a file
          int spaceIndex = line.IndexOf(' ');
          string sizeStr = line.Substring(0, spaceIndex);
          size = Convert.ToInt32(sizeStr);
          name = line.Substring(spaceIndex+1, line.Length-1-spaceIndex);
        }
        string fullName = "";
        foreach(string directory in currentDirectory) {
          fullName = fullName + directory + "/";
        }
        fullName = fullName + name;
        directorySizes.Add(fullName, size);
        
        
      }
    } // end foreach
    Console.WriteLine();
    foreach(KeyValuePair<string, int> pair in directorySizes) {
      Console.WriteLine("{0}:{1}", pair.Key, pair.Value);
    }
    
  }

  public static int CountDepth(string dir)
    {
        int count = -2;
        for (int i = 0; i < dir.Length; i++)
        {
            if (dir[i] == '/') ++count;
        }
        return count;
    }
  
  public void CalculateChildren() {
    foreach(KeyValuePair<string, int> current in directorySizes) {
      List<string> newChildrenList = new List<string>();
      directoriesAndChildren.Add(current.Key, newChildrenList);

      foreach(KeyValuePair<string, int> child in directorySizes) {
        // Only get immediate children
        var dirDepth = CountDepth(current.Key);
        var childDepth = CountDepth(child.Key);
        //Console.WriteLine("P: {0}({1}) \tC: {2}({3})", current.Key, dirDepth, child.Key, childDepth);
        if (child.Key.Contains(current.Key) && 
            (child.Key.Length > current.Key.Length) && 
            (childDepth == (dirDepth + 1))) {
          // Child is a subdirectory
          // Child is not itself
          directoriesAndChildren[current.Key].Add(child.Key);
        }
      } 
      //Console.WriteLine("{0}:{1}", pair.Key, pair.Value);
    }

    Console.WriteLine("\nHierarchy");
    foreach(KeyValuePair<string, List<string>> pair in directoriesAndChildren) {
      Console.WriteLine(pair.Key);
      foreach(string s in pair.Value) {
        Console.WriteLine("\t{0}", s);
      }
    }
    Console.WriteLine("End of hierarchy");
  }


  public int CalculateSize(string dir) {
    Console.WriteLine("Calculating the size of {0}, it's currently {1}", dir, directorySizes[dir]);
    if (directorySizes[dir] > -1) {
      return directorySizes[dir];
    } else {
      int size = 0;
      Console.WriteLine("\tCalculating children of {0}", dir);
      foreach(string child in directoriesAndChildren[dir]) {
        int newSize = CalculateSize(child);
        Console.WriteLine("\t\tAdding {0} \t{1} to {2}", child, directorySizes[child], dir);
        size += newSize;
      }
      directorySizes[dir] = size;
      Console.WriteLine("\tTotal size of {0} is {1}", dir,size);
      return size;
    }
  }

  public void CalculateAllSizes () {
    foreach(KeyValuePair<string, int> dir in directorySizes) {
      directorySizes[dir.Key] = CalculateSize(dir.Key);
    }
    Console.WriteLine();
    int directoriesAtMost100k = 0; 
    foreach(KeyValuePair<string, int> dir in directorySizes) {
      if (directoriesAndChildren[dir.Key].Count > 0) {
        // It has children
        // It's a directory
        if (dir.Value <= 100000) {
          // small enough
          Console.WriteLine("{0} has {1} children, adding {2}", dir.Key, directoriesAndChildren[dir.Key].Count, dir.Value);
          directoriesAtMost100k += dir.Value;
        }
      }
      Console.WriteLine("{0}:{1}", dir.Key, dir.Value);
    }
    Console.WriteLine(directoriesAtMost100k);
  }
}

class Program {

  public static void Main (string[] args) {
    string[] commands = System.IO.File.ReadAllLines(@"input.txt");
    Directory dir = new Directory(commands);
    dir.CalculateChildren();
    dir.CalculateAllSizes();
  }
}