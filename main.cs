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

// Need to create a parent children class
class NodeChildrenPair
{
  private string node;
  private List<Node> children;

  public NodeChildrenPair(string n, List<Node> l) {
    node = n;
    children = l;
  }
  public string getNode() {return node;}
  public List<Node> getChildren() {return children;}
}

class Program {
  // Calculate the subtree associated with the parent
  public static int CalculateSize(List<Node> nodes, IDictionary<string, int> nodeSizes, string parent) {
    int dirSize = 0;
    foreach(Node node in nodes) {
      if (node.getParent() == parent) {
        int size = node.getSize();
        if (size == -1) {
          //dirSize += CalculateSize(nodes, node.getNode()); 
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

    
    List<NodeChildrenPair> parentChildren = calculateParentChildLists(nodes);
    IDictionary<string, int> nodeSizes = new Dictionary<string, int>();
    foreach(NodeChildrenPair node in parentChildren) {
      nodeSizes.Add(node.getNode(), node.getChildren().Count);
    }

    foreach(KeyValuePair<string, int> pair in nodeSizes)
    {
      Console.WriteLine("{0}:{1}", pair.Key, pair.Value);
    }

    //foreach(NodeChildrenPair node in parentChildren) {
    //  if (size == -1) {
    //      nodeSizes[node.getNode()] = CalculateSize(nodes, node.getNode()); 
    //    }
    //  }
    //}
  

    //
    //int totalSizeUnder100k = 0;
    //foreach(NodeChildrenPair node in parentChildren) {
    //  Console.WriteLine("{0}: has {1} children", node.getNode(), node.getChildren().Count);
      // Calculate the size of each parent
    //  int size = CalculateSize(nodes, node.getNode());
      //Console.WriteLine("Parent: {0}", parent);
    //  totalSizeUnder100k += size;
    //}
    //Console.WriteLine("Total under 100k: {0}", totalSizeUnder100k);
  }

  public static List<NodeChildrenPair> calculateParentChildLists(List<Node> nodes) {
    // Find all unique parents
    HashSet<string> uniqueParents = new HashSet<string>();
    foreach (Node node in nodes) {
      uniqueParents.Add(node.getParent());
    }
    List<NodeChildrenPair> parentAndChildrenUnsorted = new List<NodeChildrenPair>();
    int biggestNumChildren = 0;
    foreach(string parent in uniqueParents) {
      List<Node> children = new List<Node>();
      foreach(Node node in nodes) {
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