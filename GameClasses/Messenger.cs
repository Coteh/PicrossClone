using System;
using System.Collections.Generic;

namespace GameClasses {
    public class MessageNode {
        private readonly List<Action> messageSubscribers;

        public MessageNode() {
            messageSubscribers = new List<Action>();
        }

        public void AddSubscriber(Action _action) {
            messageSubscribers.Add(_action);
        }
        public void RemoveSubscriber(Action _action) {
            messageSubscribers.Remove(_action);
        }

        public void InvokeSubscribers() {
            foreach (Action subAction in messageSubscribers) {
                if (subAction != null)
                    subAction();
            }
        }
    }
    public class Messenger {
        private readonly Dictionary<string, MessageNode> messageDic;

        public Messenger() {
            messageDic = new Dictionary<string, MessageNode>();
        }

        public void AddMessageAction(string _messageName, Action _action) {
            MessageNode msgNode;
            if (!messageDic.TryGetValue(_messageName, out msgNode)){
                msgNode = new MessageNode();
                messageDic.Add(_messageName, msgNode);
            }
            msgNode.AddSubscriber(_action);
        }

        public void RemoveMessageAction(string _messageName, Action _action) {
            MessageNode msgNode;
            if (!messageDic.TryGetValue(_messageName, out msgNode)) {
                Console.WriteLine("Message node {0} does not exist.", _messageName);
                return; //message node does not exist
            }
            msgNode.RemoveSubscriber(_action);
        }

        public void CallMessage(string _messageName) {
            MessageNode msgNode;
            if (!messageDic.TryGetValue(_messageName, out msgNode)) {
                Console.WriteLine("Message node {0} does not exist.", _messageName);
                return; //message node does not exist
            }
            msgNode.InvokeSubscribers();
        }
    }
}
