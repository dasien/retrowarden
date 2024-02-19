using Terminal.Gui;
using Retrowarden.Models;
using Terminal.Gui.Trees;

namespace Retrowarden.Utils
{
    public static class ViewUtils
    {
        public static T CreateControl<T>(int x, int y, int width, int height, int xFillMargin, int yFillMargin, 
            bool canFocus, bool visible, string? text, string? controlName, TextAlignment alignment=TextAlignment.Left)
            where T : View
        {
            View control = (View)Activator.CreateInstance(typeof(T));

            if (control != null)
            {
                // Assign common properties.
                control.X = x;
                control.Y = y;
                control.Width = width == 0 ? Dim.Fill(xFillMargin) : width;
                control.Height = height == 0 ? Dim.Fill(yFillMargin) : height;
                control.CanFocus = canFocus;
                control.Visible = visible;
                control.Text = text == null ? "" : text;
                control.Data = controlName == null ? "" : controlName;
                control.TextAlignment = alignment;
            }

            return (T)control;
        }

        public static TreeNode CreateCollectionsNode(List<VaultCollection> collection, SortedDictionary<string, VaultItem> items)
        {
            // Root node.
            TreeNode root = new TreeNode("Collections");
            
            // Assign type.
            root.Tag = new Tuple<NodeType, string>(NodeType.Collection, null);
            
            // Loop through the collections list.
            foreach (VaultCollection col in collection)
            {
                // Create new branch node.
                TreeNode branch = new TreeNode(col.Name);
                branch.Tag = new Tuple<NodeType, string>(NodeType.Collection, col.Id);

                // Now loop through the items.
                foreach (VaultItem item in items.Values)
                {
                    // Check to see if the item is part of a collection.
                    if (item.CollectionIds != null)
                    {
                        // Now loop through collection associations.
                        foreach (string id in item.CollectionIds)
                        {
                            // Check to see if it matches branch id.
                            if (id == col.Id)
                            {
                                // Add leaf node.
                                TreeNode leaf = new TreeNode(item.ItemName);
                                leaf.Tag = new Tuple<NodeType, string>(NodeType.Item, item.Id);
                                branch.Children.Add(leaf);
                            }
                        }
                    }
                }
                                    
                // Add branch.
                root.Children.Add(branch);
            }
            
            // Return tree.
            return root;
        }
        
        public static TreeNode CreateFoldersNode(List<VaultFolder> folders, SortedDictionary<string, VaultItem> items)
        {
            // Root node.
            TreeNode root = new TreeNode("Folders");
            root.Tag = new Tuple<NodeType, string>(NodeType.Folder, null);
            
           // Loop through the folders list.
            foreach (VaultFolder folder in folders)
            {
                TreeNode branch = new TreeNode(folder.Name);
                branch.Tag = new Tuple<NodeType, string>(NodeType.Folder, folder.Id);

                // Now loop through the items.
                foreach (VaultItem item in items.Values)
                {
                    // Check to see if a folder was set.
                    if (item.FolderId != null)
                    {
                        // Check to see if it matches branch id.
                        if (item.FolderId == folder.Id)
                        {
                            // Add leaf node.
                            TreeNode leaf = new TreeNode(item.ItemName);
                            leaf.Tag = new Tuple<NodeType, string>(NodeType.Item, item.Id);
                            branch.Children.Add(leaf);
                        }
                    }

                    else 
                    {
                        if (folder.Name == "No Folder")
                        {
                            // Add leaf node.
                            TreeNode leaf = new TreeNode(item.ItemName);
                            leaf.Tag = new Tuple<NodeType, string>(NodeType.Item, item.Id);
                            branch.Children.Add(leaf);
                        }
                    }
                }
                
                // Add branches.
                root.Children.Add(branch);
            }
            
            // Return tree.
            return root;
        }

        public static TreeNode CreateAllItemsNodes(SortedDictionary<string, VaultItem> items)
        {
            // Return value.
            TreeNode retVal = new TreeNode("All Items");
            retVal.Tag = new Tuple<NodeType, string>(NodeType.ItemGroup, null);
            // Branch nodes.
            TreeNode favorites = new TreeNode("Favorites");
            favorites.Tag = new Tuple<NodeType, string>(NodeType.Favorite, null);
            
            TreeNode logins = new TreeNode("Logins");
            logins.Tag = new Tuple<NodeType, string>(NodeType.ItemGroup, null);
            
            TreeNode cards = new TreeNode("Cards");
            cards.Tag = new Tuple<NodeType, string>(NodeType.ItemGroup, null);
            
            TreeNode identities = new TreeNode("Identities");
            identities.Tag = new Tuple<NodeType, string>(NodeType.ItemGroup, null);
            
            TreeNode notes = new TreeNode("Secure Notes");
            notes.Tag = new Tuple<NodeType, string>(NodeType.ItemGroup, null);
            
            // Loop through the item list.
            foreach (VaultItem item in items.Values)
            {
                // Create node for this item.
                TreeNode leaf = new TreeNode(item.ItemName);
                leaf.Tag = new Tuple<NodeType, string>(NodeType.Item, item.Id);

                // Check to see if it is a favorite.
                if (item.IsFavorite)
                {
                    favorites.Children.Add(leaf);
                }
                
                // Sort items based on type.
                switch (item.ItemType)
                {
                    // Login
                    case 1:
                        
                        // Add to branch.
                        logins.Children.Add(leaf);
                        break;
                    
                    // Card
                    case 2:
                        
                        // Add to branch.
                        cards.Children.Add(leaf);
                        break;
                    
                    // Identity
                    case 3:
                        
                        // Add to branch.
                        identities.Children.Add(leaf);
                        break;

                    // Secure Note
                    case 4:
                        
                        // Add to branch.
                        notes.Children.Add(leaf);
                        break;
                }
            }
            
            // Add nodes to root.
            retVal.Children.Add(favorites);
            retVal.Children.Add(logins);
            retVal.Children.Add(cards);
            retVal.Children.Add(identities);
            retVal.Children.Add(notes);

            // Return tree.
            return retVal;
        }
    }
}