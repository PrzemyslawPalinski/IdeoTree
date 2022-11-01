using IdeoTreeAPI.DTOs;
using IdeoTreeAPI.Helpers;
using IdeoTreeAPI.Model;

namespace IdeoTreeAPI.Interfaces
{
    public interface ITreeNode
    {
        Task<NodeDTO> AddTreeFromHead();
        Task<NodeDTO> AddNodeToTree(string data, int parentId);
        Task<NodeDTO> GetTreeFormDB();
        Task<NodeDTO> UpdateTreeNode(string data, int nodeToUpdate);
        Task<NodeDTO> DeleteTreeNode(int nodeToDelete);
        Task<NodeDTO> MoveNode(int nodeToMove, int newNodeParent);
    }
}
