using System;
using System.Collections.Generic;

class Directory
{
  public List<Node> nodes; // Modifiable
  public List<NodeChildrenPair> parentChildren; // Modifiable

  public Directory(List<NodeChildrenPair> pc) {
    //nodes = n;
    parentChildren = pc;
  }
  // Calculate the subtree associated with the parent
  public int CalculateSizeRecursive(NodeChildrenPair node) {
    //if (node.getChildren().Count == 0)
        return 0;}
  
  public int CalculateSize() {
    int totalSizeUnder100k = 0;
    foreach(NodeChildrenPair node in parentChildren) {
      //Console.WriteLine("{0}: has {1} children", node.getNode(), node.getChildren().Count);
      // Calculate the size of each parent
      int size =  CalculateSizeRecursive(node.getNode());
      //Console.WriteLine("Parent: {0}", parent);
      totalSizeUnder100k += size;
    }
    Console.WriteLine("Total under 100k: {0}", totalSizeUnder100k);
    return 0;
  }

  
}

class TreeNode {
  private string name;
  private int size;
  private List<TreeNode> childList;

  public TreeNode(string n, int s, List<TreeNode> c) {
    name = n;
    size = s;
    childList = c;
  }
}
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

// Need to create a parent children class
class NodeChildrenPair
{
  private Node node;
  private List<Node> children;

  public NodeChildrenPair(Node n, List<Node> l) {
    node = n;
    children = l;
  }
  public string getNode() {return node.getNode();}
  public List<Node> getChildren() {return children;}
}

class Program {


  /*public static List<NodeChildrenPair> calculateParentChildLists() {
    // Find all unique parents
    HashSet<string> uniqueParents = new HashSet<string>();
    foreach (Node node in NODES) {
      uniqueParents.Add(node.getParent());
    }
    List<NodeChildrenPair> parentAndChildrenUnsorted = new List<NodeChildrenPair>();
    int biggestNumChildren = 0;
    foreach(string parent in uniqueParents) {
      List<Node> children = new List<Node>();
      foreach(Node node in NODES) {
        if (node.getParent() == parent){
          children.Add(node);
        }
      }
      if (children.Count > 0) {
        if (children.Count>biggestNumChildren) {biggestNumChildren=children.Count;}
        NodeChildrenPair newPair = new NodeChildrenPair(parent, children);
        parentAndChildrenUnsorted.Add(newPair);
      }
    }

    // Sort parent and children, shortest num of children first
    List<NodeChildrenPair> parentAndChildrenSorted = new List<NodeChildrenPair>();
    for(int i=0; i<=biggestNumChildren; i++) {
      foreach(NodeChildrenPair pair in parentAndChildrenUnsorted) {
        if (pair.getChildren().Count == i) {
          parentAndChildrenSorted.Add(pair);
        }
      }
    }
    return parentAndChildrenSorted;
  }*/



  // Calculate the size of a directory defined by a list of nodes. 
  public static void CalculateAllSizes(List<NodeChildrenPair> parentsAndChildren) {

    // Create a directory
    Directory dir = new Directory(parentsAndChildren);
    //int under100k = dir.CalculateSize();
  }


  public static void DisplayParentChildPairs(List<NodeChildrenPair> parentChildPairs) {
    foreach (NodeChildrenPair pair in parentChildPairs) {
      Console.WriteLine("Parent: {0}", pair.getNode().getNode());
      foreach (Node node in pair.getChildren()) {
        Console.WriteLine("\t{0}", node.getNode());
      }
    }
  }

  
  // Calculates a list of NodeChildrenPair from a list of Nodes
  public static List<NodeChildrenPair> calculateParentChildLists(List<Node> nodes) {
    // Find all unique parents
    HashSet<string> uniqueParents = new HashSet<string>();
    foreach (Node node in nodes) {
      uniqueParents.Add(node.getParent());
    }
    
    List<NodeChildrenPair> parentAndChildren = new List<NodeChildrenPair>();
    // For each parent
    foreach(string parent in uniqueParents) {
      List<Node> children = new List<Node>();
      foreach(Node node in nodes) {
        if (node.getParent() == parent){
          children.Add(node);
        }
      }
      NodeChildrenPair newPair = new NodeChildrenPair(parent, children);
      parentAndChildren.Add(newPair);
    }
    return parentAndChildren;
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

    // Parsed all commands. Have a list of Nodes
    
    List<NodeChildrenPair> parentChildPairs = calculateParentChildLists(nodes);

    DisplayParentChildPairs(parentChildPairs);
    //CalculateAllSizes(parentChildPairs);
  }
}