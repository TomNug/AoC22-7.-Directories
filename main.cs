using System;
using System.Collections.Generic;


class Node
{
  private string node;
  private string parent;
  private int size;
  
  public Node(string n, string p, int si) {
    node = n;
    parent = p;
    size = si;
  }      
  public string getNode() {return node;}
  public string getParent() {return parent;}
  public int getSize() {return size;}
}


class Program {
  // Calculate the subtree associated with the parent
  public static int CalculateSize(List<Node> nodes, string parent) {
    int dirSize = 0;
    foreach(Node node in nodes) {
      if (node.getParent() == parent) {
        int size = node.getSize();
        if (size == -1) {
          dirSize += CalculateSize(nodes, node.getNode()); 
        } else {
          dirSize += size;
        }
      }
    }
    if (dirSize <= 100000) {
      return dirSize;
    } else {
      return 0;
    }
  }

  // Calculate the entire directory
  public static void CalculateAllSizes(List<Node> nodes) {
    foreach(Node node in nodes) {
      Console.WriteLine("Node: {0} Parent:{1} Size:{2}", node.getNode(), node.getParent(), node.getSize());
    }
    // Find all unique parents
    HashSet<string> uniqueParents = new HashSet<string>();
    foreach (Node node in nodes) {
      uniqueParents.Add(node.getParent());
    }
    int totalSizeUnder100k = 0;
    foreach(string parent in uniqueParents) {
      // Calculate the size of each parent
      //int size = CalculateSize(nodes, parent);
      Console.WriteLine("Parent: {0}", parent);
      //totalSizeUnder100k += size;
    }
    Console.WriteLine("Total under 100k: {0}", totalSizeUnder100k);
  }
  
  public static void Main (string[] args) {
    string[] lines = System.IO.File.ReadAllLines(@"input.txt");

    List<Node> nodes = new List<Node>();
    Node newNode = new Node("/", "Root", -1);
    string currentParent = "";
    foreach(string line in lines) {
      
      // Commands
      if (line[0] == '$') {
        // cd
        if (line[2] == 'c') {
          if (line[5] == '.'){
            // ..
            // return to parent
          } else {
            // cd b 
            // change directory
            string dir = line.Substring(5, line.Length-5);
            currentParent = dir;
          } 
        }
      } else if (line[0] == 'd') {
        // dir d 
        // make a directory
        string name = line.Substring(4, line.Length-4);
        newNode = new Node(name, currentParent, -1);
        nodes.Add(newNode);
      } else {
        // make a file
        int spaceIndex = line.IndexOf(' ');
        string sizeStr = line.Substring(0, spaceIndex);
        int size = Convert.ToInt32(sizeStr);
        string name = line.Substring(spaceIndex+1, line.Length-1-spaceIndex);
        newNode = new Node(name, currentParent, size);
        nodes.Add(newNode);
      }
    } // end foreach
    CalculateAllSizes(nodes);
  }
}