using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ziu
{
    public abstract class MonoBehaviourRequire<T> : MonoBehaviour
        where T : Component {
        
        protected T _t {
            get {
                if (!__t) {
                    __t = GetComponent<T>();
                    Debug.Assert(__t);
                }
                return __t;
            }
        }

        private T __t;
    }

    public abstract class MonoBehaviourRequire<T, T2> : MonoBehaviour
        where T : Component
        where T2 : Component
    {
        protected T _t {
            get {
                if (!__t) {
                    __t = GetComponent<T>();
                    Debug.Assert(__t);
                }
                return __t;
            }
        }
        protected T2 _t2 {
            get {
                if (!__t2) {
                    __t2 = GetComponent<T2>();
                    Debug.Assert(__t2);
                }
                return __t2;
            }
        }

        private T __t;
        private T2 __t2;
    }
}
