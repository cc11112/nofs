/*
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at 
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *  
 *  Copyright © 2011 Sebastian Ritter
 */
using System;
using System.Text;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.net
{
    [Serializable]
    public class URI : java.io.Serializable // java.lang.Comparable<URI>
    {

        internal const String unreserved = "_-!.~\'()*"; //$NON-NLS-1$

        internal const String punct = ",;:$&+="; //$NON-NLS-1$

        internal const String reserved = punct + "?/[]@"; //$NON-NLS-1$

        internal const String someLegal = unreserved + punct;

        internal const String queryLegal = unreserved + reserved + "\\\""; //$NON-NLS-1$
    
        internal const String allLegal = unreserved + reserved;
        private readonly System.Uri delegateInstance;
        /**
         * Creates a new URI instance according to the given string {@code uri}.
         *
         * @param uri
         *            the textual URI representation to be parsed into a URI object.
         * @throws URISyntaxException
         *             if the given string {@code uri} doesn't fit to the
         *             specification RFC2396 or could not be parsed correctly.
         */
        public URI(String uri) {//throws URISyntaxException 
            try {
                delegateInstance = new Uri (uri);
            }
            catch (ArgumentNullException ane) {
                throw new java.lang.NullPointerException(ane.getMessage());
            }
            catch (UriFormatException ufe) {
                throw new URISyntaxException (uri,ufe.Message);
            }
        }

        /**
         * Creates a new URI instance using the given arguments. This constructor
         * first creates a temporary URI string from the given components. This
         * string will be parsed later on to create the URI instance.
         * <p>
         * {@code [scheme:]scheme-specific-part[#fragment]}
         *
         * @param scheme
         *            the scheme part of the URI.
         * @param ssp
         *            the scheme-specific-part of the URI.
         * @param frag
         *            the fragment part of the URI.
         * @throws URISyntaxException
         *             if the temporary created string doesn't fit to the
         *             specification RFC2396 or could not be parsed correctly.
         */
        public URI(String scheme, String ssp, String frag){
                //throws URISyntaxException {
            StringBuilder uri = new StringBuilder();
            if (scheme != null) {
                uri.append(scheme);
                uri.append(':');
            }
            if (ssp != null) {
                // QUOTE ILLEGAL CHARACTERS
                uri.append(quoteComponent(ssp, allLegal));
            }
            if (frag != null) {
                uri.append('#');
                // QUOTE ILLEGAL CHARACTERS
                uri.append(quoteComponent(frag, allLegal));
            }
            try {
                delegateInstance = new Uri (uri.ToString());
            }
            catch (ArgumentNullException ane) {
                throw new java.lang.NullPointerException(ane.getMessage());
            }
            catch (UriFormatException ufe) {
                throw new URISyntaxException (uri.ToString(),ufe.Message);
            }
        }

        /**
         * Creates a new URI instance using the given arguments. This constructor
         * first creates a temporary URI string from the given components. This
         * string will be parsed later on to create the URI instance.
         * <p>
         * {@code [scheme:][user-info@]host[:port][path][?query][#fragment]}
         *
         * @param scheme
         *            the scheme part of the URI.
         * @param userinfo
         *            the user information of the URI for authentication and
         *            authorization.
         * @param host
         *            the host name of the URI.
         * @param port
         *            the port number of the URI.
         * @param path
         *            the path to the resource on the host.
         * @param query
         *            the query part of the URI to specify parameters for the
         *            resource.
         * @param fragment
         *            the fragment part of the URI.
         * @throws URISyntaxException
         *             if the temporary created string doesn't fit to the
         *             specification RFC2396 or could not be parsed correctly.
         */
        public URI(String scheme, String userinfo, String host, int port,
                String path, String query, String fragment){
                //throws URISyntaxException {
            StringBuilder uri = new StringBuilder();
            try {

                if (scheme == null && userinfo == null && host == null && path == null
                        && query == null && fragment == null) {
                delegateInstance = new Uri (uri.ToString());
                    return;
                }

                if (scheme != null && path != null && path.length() > 0
                        && path.charAt(0) != '/') {
                    throw new URISyntaxException(path, "Relative path"); //$NON-NLS-1$
                }

                if (scheme != null) {
                    uri.append(scheme);
                    uri.append(':');
                }

                if (userinfo != null || host != null || port != -1) {
                    uri.append("//"); //$NON-NLS-1$
                }

                if (userinfo != null) {
                    // QUOTE ILLEGAL CHARACTERS in userinfo
                    uri.append(quoteComponent(userinfo, someLegal));
                    uri.append('@');
                }

                if (host != null) {
                    // check for ipv6 addresses that hasn't been enclosed
                    // in square brackets
                    if (host.indexOf(':') != -1 && host.indexOf(']') == -1
                            && host.indexOf('[') == -1) {
                        host = "[" + host + "]"; //$NON-NLS-1$ //$NON-NLS-2$
                    }
                    uri.append(host);
                }

                if (port != -1) {
                    uri.append(':');
                    uri.append(port);
                }

                if (path != null) {
                    // QUOTE ILLEGAL CHARS
                    uri.append(quoteComponent(path, "/@" + someLegal)); //$NON-NLS-1$
                }

                if (query != null) {
                    uri.append('?');
                    // QUOTE ILLEGAL CHARS
                    uri.append(quoteComponent(query, allLegal));
                }

                if (fragment != null) {
                    // QUOTE ILLEGAL CHARS
                    uri.append('#');
                    uri.append(quoteComponent(fragment, allLegal));
                }
                delegateInstance = new Uri (uri.ToString());
            }
            catch (ArgumentNullException ane) {
                throw new java.lang.NullPointerException(ane.getMessage());
            }
            catch (UriFormatException ufe) {
                throw new URISyntaxException (uri.ToString(),ufe.Message);
            }
        }

        /**
         * Creates a new URI instance using the given arguments. This constructor
         * first creates a temporary URI string from the given components. This
         * string will be parsed later on to create the URI instance.
         * <p>
         * {@code [scheme:]host[path][#fragment]}
         *
         * @param scheme
         *            the scheme part of the URI.
         * @param host
         *            the host name of the URI.
         * @param path
         *            the path to the resource on the host.
         * @param fragment
         *            the fragment part of the URI.
         * @throws URISyntaxException
         *             if the temporary created string doesn't fit to the
         *             specification RFC2396 or could not be parsed correctly.
         */
        public URI(String scheme, String host, String path, String fragment) : this(scheme, null, host, -1, path, null, fragment){
            //    throws URISyntaxException {
            
        }

        /**
         * Creates a new URI instance using the given arguments. This constructor
         * first creates a temporary URI string from the given components. This
         * string will be parsed later on to create the URI instance.
         * <p>
         * {@code [scheme:][//authority][path][?query][#fragment]}
         *
         * @param scheme
         *            the scheme part of the URI.
         * @param authority
         *            the authority part of the URI.
         * @param path
         *            the path to the resource on the host.
         * @param query
         *            the query part of the URI to specify parameters for the
         *            resource.
         * @param fragment
         *            the fragment part of the URI.
         * @throws URISyntaxException
         *             if the temporary created string doesn't fit to the
         *             specification RFC2396 or could not be parsed correctly.
         */
        public URI(String scheme, String authority, String path, String query,
                String fragment)// throws URISyntaxException 
        {
            StringBuilder uri = new StringBuilder();
            try {
                if (scheme != null && path != null && path.length() > 0
                        && path.charAt(0) != '/') {
                    throw new URISyntaxException(path, "Relative path"); //$NON-NLS-1$
                }

                if (scheme != null) {
                    uri.append(scheme);
                    uri.append(':');
                }
                if (authority != null) {
                    uri.append("//"); //$NON-NLS-1$
                    // QUOTE ILLEGAL CHARS
                    uri.append(quoteComponent(authority, "@[]" + someLegal)); //$NON-NLS-1$
                }

                if (path != null) {
                    // QUOTE ILLEGAL CHARS
                    uri.append(quoteComponent(path, "/@" + someLegal)); //$NON-NLS-1$
                }
                if (query != null) {
                    // QUOTE ILLEGAL CHARS
                    uri.append('?');
                    uri.append(quoteComponent(query, allLegal));
                }
                if (fragment != null) {
                    // QUOTE ILLEGAL CHARS
                    uri.append('#');
                    uri.append(quoteComponent(fragment, allLegal));
                }
                delegateInstance = new Uri (uri.ToString());
            }
            catch (ArgumentNullException ane) {
                throw new java.lang.NullPointerException(ane.getMessage());
            }
            catch (UriFormatException ufe) {
                throw new URISyntaxException (uri.ToString(),ufe.Message);
            }
        }

        /*
         * Quote illegal chars for each component, but not the others
         * 
         * @param component java.lang.String the component to be converted @param
         * legalset java.lang.String the legal character set allowed in the
         * component s @return java.lang.String the converted string
         */
        private String quoteComponent(String component, String legalset) {
            try {
                /*
                 * Use a different encoder than URLEncoder since: 1. chars like "/",
                 * "#", "@" etc needs to be preserved instead of being encoded, 2.
                 * UTF-8 char set needs to be used for encoding instead of default
                 * platform one
                 */
                return URIEncoderDecoder.quoteIllegal(component, legalset);
            } catch (java.io.UnsupportedEncodingException e) {
                throw new java.lang.RuntimeException(e.toString());
            }
        }
        /**
         * Parses the given argument {@code uri} and creates an appropriate URI
         * instance.
         *
         * @param uri
         *            the string which has to be parsed to create the URI instance.
         * @return the created instance representing the given URI.
         */
        public static URI create(String uri) {
            URI result = null;
            try {
                result = new URI(uri);
            } catch (URISyntaxException e) {
                throw new java.lang.IllegalArgumentException(e.getMessage());
            }
            return result;
        }

        /**
         * Returns the textual string representation of this URI instance.
         *
         * @return the textual string representation of this URI.
         */
        public override String ToString() {
            return this.delegateInstance.ToString();
        }

        /**
         * Converts this URI instance to a URL.
         *
         * @return the created URL representing the same resource as this URI.
         * @throws MalformedURLException
         *             if an error occurs while creating the URL or no protocol
         *             handler could be found.
         */
        public URL toURL() {//throws MalformedURLException {
            if (!this.delegateInstance.IsAbsoluteUri) {
                throw new java.lang.IllegalArgumentException("URI is not absolute: "+ToString());
            }
            return new URL(ToString());
        }

    }
}
