using IdeoTreeAPI.Data;
using IdeoTreeAPI.DTOs;
using IdeoTreeAPI.Helpers;
using IdeoTreeAPI.Interfaces;
using IdeoTreeAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdeoTreeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeNodeController : ControllerBase
    {
        ITreeNode _treeNode;
        public TreeNodeController(ITreeNode treeNode)
        {
            _treeNode = treeNode;
        }

        [Route("create")]
        [HttpPost]
        public async Task<ActionResult<NodeDTO>> CreateTree()
        {

            NodeDTO treeDb = await _treeNode.AddTreeFromHead();
            if (treeDb == null)
            {
                return Conflict("Tree already exists");
            }

            return Ok(treeDb);
        }

        [Route("add")]
        [HttpPost]
        public async Task<ActionResult<NodeDTO>> AddNode(string data, int id)
        {
            var nodeToReturn = await _treeNode.AddNodeToTree(data, id);
            if (nodeToReturn == null) BadRequest("Parent node does not exist");
            return Ok(nodeToReturn);
        }

        [HttpGet]
        public async Task<ActionResult<NodeDTO>> GetTree()
        {
            var root = await _treeNode.GetTreeFormDB();
            return Ok(root);
        }

        [Route("{nodeId:int}")]
        [HttpPut]
        public async Task<ActionResult<NodeDTO>> UpdateNode(string data, int nodeId)
        {
            var updated = await _treeNode.UpdateTreeNode(data, nodeId);
            return Ok(updated);
        }

        [Route("{nodeId:int}")]
        [HttpDelete]
        public async Task<ActionResult<NodeDTO>> DeleteNode(int nodeId)
        {
            var node = await _treeNode.DeleteTreeNode(nodeId);
            return Ok(node);
        }

        [Route("{nodeToMove:int}/{newNodeParent:int}")]
        [HttpPut]
        public async Task<ActionResult<NodeDTO>> MoveNode(int nodeToMove, int newNodeParent)
        {
            var updated = await _treeNode.MoveNode(nodeToMove, newNodeParent);
            if (updated == null) return BadRequest("Node is root or one of them does not exist");
            return Ok(updated);
        }
    }
}
