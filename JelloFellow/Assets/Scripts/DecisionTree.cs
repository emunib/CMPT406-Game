using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour {

    //Delegate templates
   
    public delegate bool Decision(); //return type bool, no params
    public delegate void Action(); //return type void, no params

    //Children
    DecisionTree rightChild;
    DecisionTree leftChild;

    //member variable declaration
    Decision decision;
    Action action;


	void Start () {
		
	}
	
	void Update () {
		
	}

    public DecisionTree(){
        decision = null;
        action = null;
        rightChild = null;
        leftChild = null;

    }

    public void SetLeftChild(DecisionTree lChild){
        leftChild = lChild;
    }

    public void SetRightChild(DecisionTree rChild){
        rightChild = rChild;
    }

    public void SetDecisionDelegate(Decision d){
        decision = d;
    }

    public void SetActionDelegate(Action a){
        action = a;
    }


    bool Decide(){
        //calls this function and returns the return value
        return decision(); 
    }

    void Yes(){
        leftChild.Search();
    }
    void No(){
        rightChild.Search();
    }

    public void Search(){
        
        if(action != null){ //or action != null
            //leaf node, execute action
            //Debug.Log("ACTION");
            action(); 
        }
        else if(Decide()){
            //go left for yes
            //Debug.Log("DECISION Yes");
            Yes();
        }
        else{
            //go right for no
            //Debug.Log("DECISION No");
            No();
        }
    }
}
