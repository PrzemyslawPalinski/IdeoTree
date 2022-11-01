using AutoMapper;
using IdeoTreeAPI.DTOs;
using IdeoTreeAPI.Helpers;
using IdeoTreeAPI.Interfaces;
using IdeoTreeAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace IdeoTreeAPI.Data
{
    public class TreeNodeRepository : ITreeNode
    {
        DataContext _context;
        IMapper _autoMapper;
        public TreeNodeRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _autoMapper = mapper;
        }

        async public Task<NodeDTO> AddNodeToTree(string data, int parentId)
        {
            List<TreeNodeDB> all = _context.TreeNodes.Include(x => x.Parent).ToList();
            var nodeToAdd = new TreeNodeDB();
            nodeToAdd.Data = data;
            var parentNode = all.First(x => x.Id == parentId);
            if (parentNode == null) return null;
            nodeToAdd.Parent = parentNode;
            nodeToAdd.ParentId = parentId;
            if (parentNode.Children == null)
            {
                parentNode.Children = new List<TreeNodeDB>();
            }
            parentNode.Children.Add(nodeToAdd);
            await _context.SaveChangesAsync();
            return _autoMapper.Map<NodeDTO>(all.Find(x => x.Parent == null));
        }

        async public Task<NodeDTO> AddTreeFromHead()
        {
            TreeExtensions.Tree<string> root = new TreeExtensions.Tree<string>("root");
            {
                TreeExtensions.Tree<string> node0 = root.AddChild("node0");
                TreeExtensions.Tree<string> node1 = root.AddChild("node1");
                TreeExtensions.Tree<string> node2 = root.AddChild("node2");
                {
                    TreeExtensions.Tree<string> node20 = node2.AddChild("node20");
                    TreeExtensions.Tree<string> node21 = node2.AddChild("node21");
                    {
                        TreeExtensions.Tree<string> node210 = node21.AddChild("node210");
                        TreeExtensions.Tree<string> node211 = node21.AddChild("node211");
                    }
                }
                TreeExtensions.Tree<string> node3 = root.AddChild("node3");
                {
                    TreeExtensions.Tree<string> node30 = node3.AddChild("node30");
                }
            }
            if (_context.TreeNodes.ToList().Count() == 0)
            {
                List<TreeNodeDB> treeNodeList = new List<TreeNodeDB>();
                foreach (TreeExtensions.ITree<string> node in root)
                {
                    TreeNodeDB treeNode = new TreeNodeDB();
                    treeNode.Data = node.Data;
                    if (node.Parent != null)
                    {
                        var parent = treeNodeList.Find(x => x.Id == node.Parent.Id);
                        treeNode.ParentId = node.Parent.Id;
                        treeNode.Parent = parent;
                    }
                    _context.TreeNodes.Add(treeNode);
                    await _context.SaveChangesAsync();
                    node.Id = treeNode.Id;
                    treeNodeList.Add(treeNode);
                }
                NodeDTO nodeDTO = _autoMapper.Map<NodeDTO>(treeNodeList.First());
                return nodeDTO;
            }
            return null;
        }

        public async Task<NodeDTO> DeleteTreeNode(int nodeToDelete)
        {
            List<TreeNodeDB> all = _context.TreeNodes.Include(x => x.Parent).ToList();
            var nodeFromDB = all.Find(x => x.Id == nodeToDelete);
            if (nodeFromDB == null) return null;
            if (nodeFromDB.Children == null)
            {
                _context.TreeNodes.Remove(nodeFromDB);             
            }
            else
            {
                foreach (var child in nodeFromDB.Children)
                {
                    await DeleteTreeNode(child.Id);
                }
                _context.TreeNodes.Remove(nodeFromDB);
            }
            await _context.SaveChangesAsync();
            return _autoMapper.Map<NodeDTO>(all.Find(x => x.Parent == null));
        }


        public async Task<NodeDTO> GetTreeFormDB()
        {
            List<TreeNodeDB> all = _context.TreeNodes.Include(x => x.Parent).ToList();
            if (all.Count() == 0)
            {
                return await AddTreeFromHead();
            }
            var root = all.Find(x => x.Parent == null);
            var nodeDTO = _autoMapper.Map<NodeDTO>(root);
            return nodeDTO;
        }

        public async Task<NodeDTO> MoveNode(int nodeToMove, int newNodeParent)
        {
            List<TreeNodeDB> all = _context.TreeNodes.Include(x => x.Parent).ToList();
            var nodeForUpdate = all.First(x => x.Id == nodeToMove);
            var newParent = all.First(x => x.Id == newNodeParent);
            if (nodeForUpdate.Parent == null || nodeForUpdate == null || newParent == null) return null;
            nodeForUpdate.Parent.Children.Remove(nodeForUpdate);
            nodeForUpdate.Parent = newParent;
            if (newParent.Children == null)
            {
                newParent.Children = new List<TreeNodeDB>();
            }
            newParent.Children.Add(nodeForUpdate);
            await _context.SaveChangesAsync();
            return _autoMapper.Map<NodeDTO>(all.Find(x => x.Parent == null));
        }

        public async Task<NodeDTO> UpdateTreeNode(string data, int nodeToUpdate)
        {
            List<TreeNodeDB> all = _context.TreeNodes.Include(x => x.Parent).ToList();
            var nodeForUpdate = all.First(x => x.Id == nodeToUpdate);
            nodeForUpdate.Data = data;
            await _context.SaveChangesAsync();
            return _autoMapper.Map<NodeDTO>(all.Find(x => x.Parent == null));
        }
    }
}
