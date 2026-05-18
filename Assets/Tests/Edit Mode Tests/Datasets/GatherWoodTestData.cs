using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP {
    public class GatherWoodTestData {

        public const int woodStockMax = 999;

        #region Items
        public Item choppedWood;
        public Item axe;
        public Item money;
        #endregion

        #region Location Types
        public LocationType tree;
        public LocationType workshop;
        public LocationType woodstock;
        public LocationType shop;
        #endregion

        #region Inventories
        public Inventory npcInventoryComponent;
        public Inventory workshopInventoryComponent;
        public Inventory woodstockInventoryComponent;
        public Inventory treeInventoryComponent;
        public Inventory shopInventoryComponent;
        #endregion

        #region Game Objects
        public GameObject npcObject;
        public GameObject treeObject;
        public GameObject workshopObject;
        public GameObject woodstockObject;
        public GameObject shopObject;
        #endregion

        #region  GOAP
        #region World States
        public G_WorldState npcWorldState;
        #endregion

        #region States
        public G_Inventory npcInventory;   
        public G_Inventory workshopInventory;   
        public G_Inventory woodStockInventory;   
        public G_Inventory treeInventory;   
        public G_Inventory shopInventory;  
        public G_AtLocation atLocation;
        #endregion 

        #region Actions

        public G_Action deliverWood; // :}
        public G_Action goToWoodStock; // :}
        public G_Action chopTree; // :}
        public G_Action goToTree; // :}
        public G_Action takeAxe; // :}
        public G_Action goToWorkshop; // :}
        public G_Action buyWood; // :}
        public G_Action goToShop; // :}

        #endregion

        #region Goals

        public G_Goal gatherWood;

        #endregion
        #endregion
        public GatherWoodTestData() {

            #region Data Creation
            choppedWood = An.Item("choppedWood").isStackable(true);
            axe = An.Item("axe").isStackable(false);
            money = An.Item("money").isStackable(true);

            tree = A.LocationType("tree");
            workshop = A.LocationType("workshop");
            woodstock = A.LocationType("woodstock");
            shop = A.LocationType("shop");
            #endregion

            #region Objects and Component Creation
            npcObject = new GameObject();
            treeObject = new GameObject();
            workshopObject = new GameObject();
            woodstockObject = new GameObject();
            shopObject = new GameObject();

            npcInventoryComponent = npcObject.AddComponent<Inventory>();
            treeInventoryComponent = treeObject.AddComponent<Inventory>();
            workshopInventoryComponent = workshopObject.AddComponent<Inventory>();
            woodstockInventoryComponent = woodstockObject.AddComponent<Inventory>();
            shopInventoryComponent = shopObject.AddComponent<Inventory>();
            #endregion

            #region State Creation
            npcInventory = An.InventoryState("npcInventory").WithInventory(npcInventoryComponent);
            treeInventory = An.InventoryState("treeInventory").WithInventory(treeInventoryComponent);
            workshopInventory = An.InventoryState("workshopInventory").WithInventory(workshopInventoryComponent);
            woodStockInventory = An.InventoryState("woodStockInventory").WithInventory(woodstockInventoryComponent);
            shopInventory = An.InventoryState("shopInventory").WithInventory(shopInventoryComponent);
            atLocation = An.AtLocation("atLocation").WithLocationType(null);
            #endregion

            #region Action Generation

            deliverWood = An.Action("deliverWood")
                .WithPrecondition(A.Condition().State(npcInventory).IsGreaterThanOrEqualTo(new ItemStack(choppedWood, 10)))

                .WithPrecondition(A.Condition().State(atLocation).IsEqualTo(woodstock))

                .WithEffect(A.Condition().State(woodStockInventory).IsEqualTo(new ItemStack(choppedWood, woodStockMax)))
                .WithEffect(A.Condition().State(npcInventory).IsEqualTo(ItemStack.EmptyStack(choppedWood)))
                
                .WithCost(10);
            

            goToWoodStock = An.Action("goToWoodStock")
                .WithEffect(A.Condition().State(atLocation).IsEqualTo(woodstock))
                
                .WithCost(10)
                .WithPriority(1);


            chopTree = An.Action("chopTree")
                .WithPrecondition(A.Condition().State(npcInventory).IsGreaterThan(ItemStack.EmptyStack(axe)))

                .WithPrecondition(A.Condition().State(atLocation).IsEqualTo(tree))

                .WithEffect(A.Condition().State(npcInventory).IsGreaterThanOrEqualTo(new ItemStack(choppedWood, 10)))
                .WithEffect(A.Condition().State(treeInventory).IsEqualTo(ItemStack.EmptyStack(choppedWood)))

                .WithCost(10);


            goToTree = An.Action("goToTree")
                .WithEffect(A.Condition().State(atLocation).IsEqualTo(tree))

                .WithCost(10)
                .WithPriority(1);
        

            takeAxe = An.Action("takeAxe")
                .WithPrecondition(A.Condition().State(npcInventory).IsEqualTo(ItemStack.EmptyStack(axe)))
                .WithPrecondition(A.Condition().State(workshopInventory).IsGreaterThan(ItemStack.EmptyStack(axe)))
                .WithPrecondition(A.Condition().State(atLocation).IsEqualTo(workshop))

                .WithEffect(A.Condition().State(npcInventory).IsGreaterThan(ItemStack.EmptyStack(axe)))
                .WithEffect(A.Condition().State(workshopInventory).IsEqualTo(ItemStack.EmptyStack(axe)))

                .WithCost(10);


            goToWorkshop = An.Action("goToWorkshop")
                .WithEffect(A.Condition().State(atLocation).IsEqualTo(workshop))

                .WithCost(10)
                .WithPriority(1);
        

            buyWood = An.Action("buyWood")
                .WithPrecondition(A.Condition().State(npcInventory).IsEqualTo(ItemStack.EmptyStack(choppedWood)))
                .WithPrecondition(A.Condition().State(npcInventory).IsGreaterThanOrEqualTo(new ItemStack(money, 1)))
                .WithPrecondition(A.Condition().State(shopInventory).IsGreaterThan(ItemStack.EmptyStack(choppedWood)))
                .WithPrecondition(A.Condition().State(atLocation).IsEqualTo(shop))

                .WithEffect(A.Condition().State(npcInventory).IsGreaterThanOrEqualTo(new ItemStack(choppedWood, 10)))
                .WithEffect(A.Condition().State(npcInventory).IsEqualTo(ItemStack.EmptyStack(money)))
                .WithEffect(A.Condition().State(shopInventory).IsEqualTo(ItemStack.EmptyStack(choppedWood)))

                .WithCost(10);


            goToShop = An.Action("goToShop")
                .WithEffect(A.Condition().State(atLocation).IsEqualTo(shop))

                .WithCost(10)
                .WithPriority(1);

            #endregion

            #region Goals

            gatherWood = A.Goal("gatherWood")
                .WithTrigger(A.Condition().State(woodStockInventory).IsLessThan(new ItemStack(choppedWood, woodStockMax)))
                .WithEffect(A.Condition().State(woodStockInventory).IsEqualTo(new ItemStack(choppedWood, woodStockMax)))

                .WithPriority(1);

            #endregion

            #region World State
            npcWorldState = ScriptableObject.CreateInstance<G_WorldState>();

            npcWorldState.states.Add(npcInventory);
            npcWorldState.states.Add(workshopInventory);
            npcWorldState.states.Add(woodStockInventory);
            npcWorldState.states.Add(treeInventory);
            npcWorldState.states.Add(shopInventory);
            npcWorldState.states.Add(atLocation);

            npcWorldState.actionPool.Add(deliverWood);
            npcWorldState.actionPool.Add(goToWoodStock);
            npcWorldState.actionPool.Add(chopTree);
            npcWorldState.actionPool.Add(goToTree);
            npcWorldState.actionPool.Add(takeAxe);
            npcWorldState.actionPool.Add(goToWorkshop);
            npcWorldState.actionPool.Add(buyWood);
            npcWorldState.actionPool.Add(goToShop);

            npcWorldState.goals.Add(gatherWood);
            #endregion
        }

        public void AddDataForTest(bool hasShop) {
            workshopInventoryComponent.AddToInventory(new ItemStack(axe, 1));
            treeInventoryComponent.AddToInventory(new ItemStack(choppedWood, 10));
            if (hasShop) {
                shopInventoryComponent.AddToInventory(new ItemStack(choppedWood, 10));
                npcInventoryComponent.AddToInventory(new ItemStack(money, 1));
            }
        }
    }
}
